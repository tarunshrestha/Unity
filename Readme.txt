Install Unity in Ubuntu:
	sudo sh -c 'echo "deb [signed-by=/usr/share/keyrings/Unity_Technologies_ApS.gpg] https://hub.unity3d.com/linux/repos/deb stable main" > /etc/apt/sources.list.d/unityhub.list'
	sudo apt-get update
	sudo apt-get install unityhub

To run:	
	unityhub --no-sandbox

Install Libraries:
	sudo apt-get install libnss3 libx11-xcb1 libxcomposite1 libxdamage1 libxrandr2

