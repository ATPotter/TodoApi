# Coded for Python 3.x
# Tested under v3.3.1

import os
import sys
import subprocess
import argparse
import time

baselinePath = os.getcwd()

if baselinePath == None:
    baselinePath = os.getcwd() # Default to executing path if env variable not set

# Execute command and print given message
def execute(cmd, msg):
    print(msg, end="")
    
    try:
        output = subprocess.check_output(cmd, universal_newlines=True, stderr=subprocess.STDOUT, shell=True)
        # If we have anything on the output it's likely to be an error or warning. Bail out.
        if output:
            print("") # Newline
            print(output)
            sys.exit(101)
    except subprocess.CalledProcessError as err:
        print("") # Newline
        print (err.output)
        
        sys.exit(102)
    
    print(' done.')

# Execute command, retry if it fails
def executeWithRetry(cmd, msg, maxRetries, delayInSeconds):

    commandExecuted = False
    
    for i in range(maxRetries):
        print ("ExecuteWithRetry: " + msg, end="")
        try:
            output = subprocess.check_output(cmd, universal_newlines=True, stderr=subprocess.STDOUT, shell=True)
            if(output):
                print ("Error reported, retrying: {}".format(output))
            else:
                # Successful completion - exit.
                commandExecuted = True
                break;
        except KeyboardInterrupt as kbd_err:
            sys.exit(200)
        except subprocess.CalledProcessError as err:
            print ("")
            print(err.output)
            #print ("err:")
            #print ("")
            #print (err)
        except:
            print ("Exception raised, retrying: {}".format(sys.exc_info()[0]));
            
        # We only get here if we either got unexpected output or we got an exception.
        # Delay and try again
        time.sleep(delayInSeconds)
    
    # All done - did we succeed?
    if(not commandExecuted):
        sys.exit(104)
    
    # We must have succeeded
    return
    
# Execute command for each file on folder
def executeOn(cmd, folder):
    folder = os.path.join(baselinePath, folder)
    for file in sorted(os.listdir(folder)):
        sqlfile = os.path.join(baselinePath, folder, file)
        if os.name == 'nt':
            sqlfile = '\"{0}\"'.format(sqlfile)
        execute(cmd+sqlfile, "Executing '" + sqlfile + "' ...")

def main():
    global baselinePath
    
    # Default parameters
    host = "localhost"
    port = (int)(os.environ.get("PGPORT", "5432"))
    postgresDbName = "postgres"
    adminUserName = "rpcadmin"
    adminPassword = adminUserName
    databaseName = "rpcdemo"
    
    # Parse command line arguments
    parser = argparse.ArgumentParser(description='Postgres schema rebuild script for rpcdemo')
    parser.add_argument('--superuser', help='Name for the Postgres superuser database role (default: postgres)', default='postgres')
    parser.add_argument('--path', help='Path to the Baseline folder (where all the scripts reside)', default=baselinePath)
    args = parser.parse_args()
    superUserName = args.superuser
    baselinePath = args.path
    
    print("---------------------------------------")
    print("Superuser:", superUserName)
    print("Assuming all files are relative to:", baselinePath)
    print("Port number ", port)
    
    # Prepare our environment variables
    os.environ['PGOPTIONS'] = '--client-min-messages=warning' # Suppress psql notice messages
    os.environ['PGPASSWORD'] = 'postgres'

    # Drop everything to start with: database and admin user
    # Note we use executeWithRetry for the first command in case the database
    # is slow to start
    executeWithRetry('dropdb -h {0} -p {1} --if-exists -U {2} {3}'.format(host, port, superUserName, databaseName), 'Dropping database {0}...'.format(databaseName), 5, 1)
    execute('dropuser -h {0} -p {1} --if-exists -U {2} {3}'.format(host, port, superUserName, adminUserName), 'Dropping user {0}...'.format(adminUserName))
    
    # Start by creating the 'admin' user
    # This is how we will identify ourselves for any subsequent commands.
    psqlFlags = ' -h {0} -p {1} -q -w -X -1 -v ON_ERROR_STOP=1 '.format(host, port)
    execute('psql {0} -U {1} -d {2} -c "CREATE USER {3} WITH CREATEDB CREATEROLE PASSWORD \'{4}\';"'.format(psqlFlags, superUserName, postgresDbName, adminUserName, adminPassword), 'Creating user {0}...'.format(adminUserName));
	
    # Now, create the database
    os.environ['PGPASSWORD'] = adminPassword
    execute('createdb -U {0} -E UTF-8 {1}'.format(adminUserName, databaseName), 'Creating database {0}...'.format(databaseName))
    
    # OK. From here on we build the database, file by file
    # Prepare our command string for psql:
    # -h, database server host
    # -p, database server port
    # -q, run quietly (no messages, only query output)
    # -w, never prompt for password
    # -f, execute commands from file, then exit
    # -X, do not read startup file (makes sure we are not affected by external config)
    # -1, execute as a single transaction
    # -v ON_ERROR_STOP=1, stop execution when a transaction fails
    # -U, our user name
    # -d, our database
    # -f, the file to run  
	
    suPsqlFlags = '{0}'.format(psqlFlags);
    suPsqlFlags += ' -U {0} -d {1} -f '.format(superUserName, databaseName)
    suCmd = "psql" + suPsqlFlags
	
    psqlFlags += ' -U {0} -d {1} -f '.format(adminUserName, databaseName)
    cmd = "psql" + psqlFlags

    # Create additional roles
    executeOn(cmd, 'Setup')

    # Create tables
    executeOn(cmd, 'Tables')

    # Create views
    executeOn(cmd, 'Views')
    
    # Populate tables
    executeOn(cmd, 'Data')

	# Apply table constraints
    executeOn(cmd, 'Constraints')
	
    # Grant the appropriate privileges for each table
    executeOn(cmd, 'Privileges')
    
    # Create functions
    executeOn(cmd, 'Functions')

    # Create triggers
    executeOn(cmd, 'Triggers')
	
	# Create special SU Functions
    os.environ['PGPASSWORD'] = 'postgres'
    executeOn(suCmd, 'SuFunctions')
	
    # Done.
    print("All steps completed successfully.")
    
# Go!
main()
