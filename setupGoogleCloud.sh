# ubuntu 20 (following instructions from https://docs.microsoft.com/en-us/dotnet/core/install/linux-ubuntu#2004)

wget https://packages.microsoft.com/config/ubuntu/20.10/packages-microsoft-prod.deb -O packages-microsoft-prod.deb
sudo dpkg -i packages-microsoft-prod.deb

sudo apt-get update; \
  sudo apt-get install -y apt-transport-https && \
  sudo apt-get update && \
  sudo apt-get install -y aspnetcore-runtime-6.0  
  
sudo apt-get install unzip zip screen

#screen -r # explanation on https://stackoverflow.com/questions/48221807/google-cloud-instance-terminate-after-close-browser

#unzip -o code_and_previously_tried_pwds.zip 

#dotnet BruteForcePasswordFinder.dll "my_keystore_file.json"

ls -l --block-size=M

zip -9 previously_tried_pwds.zip ./files