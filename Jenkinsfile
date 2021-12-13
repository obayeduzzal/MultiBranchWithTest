def flag = false;
pipeline{
    agent {
        label {
            label ""
            customWorkspace "C:/JenkinsData/${JOB_NAME}"
        }
    }
    options {
        timeout(time: 30, unit: 'MINUTES')
    }
    stages{
        stage('Job Start Notification'){
			steps{
				echo 'Starting Job'
				emailext(
						to: 'obayed.khandaker@brainstation-23.com',
						subject: "Job Started '${env.JOB_NAME}', Build#${env.BUILD_NUMBER}",
						body: "Job Started '${env}'"
					)
			}          
        }
        stage('Restore'){
            steps{
                //options{
                    //timeout(time: 1, unit: 'HOURS')
                //}
                echo 'Restoring Missing Packages'
                bat 'dotnet restore'
            }
        }
        stage('Clean'){
            steps{
                echo 'Cleaning Solution'
                bat 'dotnet clean'
            }
        }
        stage('Build'){
            steps{
                echo 'Building Solution'
                bat 'dotnet build'
            }
        }
        stage('Unit Test'){
            steps{
                echo 'Running Unit Test'
                bat 'dotnet test --collect:"XPlat Code Coverage"'
            }
        }
		stage('Test Report Generation'){
			steps{
				echo 'Generating Test Report'
				bat 'reportgenerator -reports:./MutliBranchWithUnit.Test/TestResults/{guid}/coverage.cobertura.xml -targetdir:./MutliBranchWithUnit.Test/Reports'
			}
		}
        stage('UAT Publish'){
            when{
                branch 'master'
            }
            steps{
                echo 'Publishing For UAT'
                bat 'dotnet publish -c release'
            }
        }
        stage('Prod Publish'){
            when{
                branch 'Production'
            }
            steps{
                echo 'Publishing For UAT'
                bat 'dotnet publish -c release'
            }
        }
        stage('DB Migration'){
            steps{
                dir('${env.WORKSPACE/CoreWithUnit.Api}'){
                    echo 'Migrating Database'
                    bat 'dotnet ef database update'
				    script { flag = true }
                }
            }
        }
        stage('Delete Previous'){
			when{
				expression { flag == true }
			}
            steps{
                bat 'rmdir /s /q "C:\\CICD\\Deployment\\MultiBranchTest"'
            }
        }
        stage('copy'){
			when{
				expression { flag == true }
			}
            steps{
                bat'robocopy C:\\JenkinsData\\MultiBrnachTest\\master\\CoreWithUnit.Api\\bin\\Release\\net5.0\\publish C:\\CICD\\Deployment\\MultiBranchTest /e & EXIT /B 0'
            }
        }
    }
    post{
        success {
            echo 'Job Succeded'
            emailext(
                    to: 'obayed.khandaker@brainstation-23.com',
                    subject: "Job Finished '${env.JOB_NAME}', Build#${env.BUILD_NUMBER}",
                    body: "Job Finished '${env}'"
                )
        }

        failure {
            echo 'Job Failed'
            emailext(
                    to: 'obayed.khandaker@brainstation-23.com',
                    subject: "Job Failed '${env.JOB_NAME}', Build#${env.BUILD_NUMBER}",
                    body: "Job Failed '${BUILD_URL}''",
                    attachLog: true
                )
        }

    }
}