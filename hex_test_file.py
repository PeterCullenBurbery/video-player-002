# hex_test_file.py
def create_test_file(filename, size=256):
    # Create a binary file with a repeating pattern
    with open(filename, 'wb') as f:
        for i in range(size):
            # Write bytes in a simple pattern (0x00, 0x01, ..., 0xFF)
            f.write(bytes([i % 256]))

if __name__ == "__main__":
    create_test_file('hex_test_file.bin')
    print("File 'hex_test_file.bin' created.")
