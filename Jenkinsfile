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
                parallel {
                    stage('Generate HTML Report'){
                        steps{
                            echo 'Printing'
                        }
                    }
                    stage('Generate Cobertura Report'){
                        steps{
                            echo 'Running Unit Test'
                            bat 'dotnet test --collect:"XPlat Code Coverage"'
                        }
                    }
                    stage('Generate Report Using Other Report'){
                        steps{
                            echo 'Printing'
                        }
                    }
                }
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
        stage('Taking Backup'){
            steps{
                parallel {
                    stage('Previous Build Backup'){
                        steps{
                            echo 'Taking Prevoius Build Backup'
                            bat 'powershell Compress-Archive C:\\CICD\\Deployment\\Pipeline C:\\CICD\\Archive\\ABC_Build_${env.BUILD_NUMBER}.zip'
                        }
                    }
                    stage('Previous Database Backup'){
                        steps{
                            echo 'Taking Previous Database Backup'
                        }
                    }
                }
            }
        }
        stage('DB Migration'){
            steps{
                echo 'Migrating Datatbase'
                script{
                    try{
                        bat 'cd MultiBranchWithTest.App && dotnet ef database update'
                        flag = true
                    } catch(error){
                        echo 'Database Migration Failed. Moving To Next Stage'
                        currentBuild.result = 'SUCCESS'
                    }
                }
            }
        }
        stage('Delete Previous Build'){
			when{
				expression { flag == true }
			}
            steps{
                bat 'rmdir /s /q "C:\\CICD\\Deployment\\MultiBranchTest"'
            }
        }
        stage('Deploy Artifacts'){
			when{
				expression { flag == true }
			}
            steps{
                bat'robocopy C:\\JenkinsData\\MultiBrnachTest\\master\\CoreWithUnit.Api\\bin\\Release\\net5.0\\publish C:\\CICD\\Deployment\\MultiBranchTest /e & EXIT /B 0'
            }
        }
        stage('Health Check'){
            steps{
                parallel {
                    stage('DB Health Check'){
                        steps{
                            echo 'Checking DB Availability'
                        }
                    }
                    stage('Project Health Check'){
                        steps{
                            echo 'Cheking Project Availability'
                        }
                    }
                }
            }
        }
    }
    post{
		always {
            step([$class: 'CoberturaPublisher', autoUpdateHealth: false, autoUpdateStability: false, coberturaReportFile: '**/coverage.cobertura.xml', failUnhealthy: false, failUnstable: false, maxNumberOfBuilds: 0, onlyStable: false, sourceEncoding: 'ASCII', zoomCoverageChart: false])
        }
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