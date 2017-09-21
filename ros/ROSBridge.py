import socket
import json
import traceback

class Vector3(object):
	"""docstring for Vector3"""
	def __init__(self, x, y, z):
		self.x = x
		self.y = y
		self.z = z

def parse_points (points):
	pointsList = []
	for point in points:
		pointsList.append(Vector3(point['x'], point['y'], point['z']))
	
	return pointsList;

def main():
	TCPSock = socket.socket(socket.AF_INET, socket.SOCK_STREAM)
	TCPSock.connect(("localhost", 8888))

	try:
		while True:
			data = TCPSock.recv(1024)
			points = json.loads(data);
			pointsList = parse_points(points['points'])
			print pointsList


	except Exception:
		traceback.print_exc()
		TCPSock.close();

if __name__ == '__main__':
	main()



