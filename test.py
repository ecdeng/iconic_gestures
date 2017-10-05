from PointStream import *

def siginit_handler(signum, frame):
	sys.exit()
signal.signal(signal.SIGINT, siginit_handler)

def main():
	global ps
	ps = PointStream()
	ps.start()
	while True:
		if not ps.empty():
			points = ps.dequeue()
			for point in points:
				print (point.toString())

if __name__ == '__main__':
	main()