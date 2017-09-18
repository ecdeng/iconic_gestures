import socket

def main():
	TCPOutSock = socket.socket(socket.AF_INET, socket.SOCK_STREAM)
	TCPOutSock.connect(("localhost", 8888))

if __name__ == '__main__':
	main()