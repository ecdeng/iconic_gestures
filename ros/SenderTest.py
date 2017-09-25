import socket
import sys
import signal
import time

def siginit_handler(signum, frame):
	sys.exit()
signal.signal(signal.SIGINT, siginit_handler)

def main():
	tcpInSock = socket.socket(socket.AF_INET, socket.SOCK_STREAM)
	tcpInSock.bind(("localhost", 8888))
	tcpInSock.listen(5)

	connectionSocket, addr = tcpInSock.accept()
	while True:
		connectionSocket.send("{\"points\":[{\"x\":1.0,\"y\":2.0,\"z\":3.0},{\"x\":4.0,\"y\":5.0,\"z\":6.0}],\"normals\":[{\"x\":0.0,\"y\":1.0,\"z\":0.0},{\"x\":1.0,\"y\":0.0,\"z\":0.0}]}")
		time.sleep(1);
if __name__ == '__main__':
	main()