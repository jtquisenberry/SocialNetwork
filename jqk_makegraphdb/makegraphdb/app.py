import sys
sys.path.append('../')
from parse_file import NetworkTextParser
from write_db import SocialNetworkWriter
#from .parse_file import  NetworkTextParser
#from .write_db import SocialNetworkWriter
import sys
import argparse


if __name__ == '__main__':

    # Setup file inputs
    entity_file = 'data/entity_properties.txt'
    relationship_file = 'data/entity_relationships.txt'
    if len(sys.argv) == 3:
        entity_file = sys.argv[1]
        relationship_file = sys.argv[2]
    if 1 < len(sys.argv) < 3:
        raise argparse.ArgumentParser('Please enter two command line arguments.')

    # This would be a good time to implement try...except to determine whether the
    # specified files exist.

    parser = NetworkTextParser()
    parser.parse_properties(entity_file)
    parser.parse_relationships(relationship_file)

    # An example of dependency injection. If I had other parsers, such as an
    # RDMS parser, I would have both NetworkTextParser and NetworkRDMSParser
    # implement a common interface.
    writer = SocialNetworkWriter(parser)
    writer.populate_nodes()
    writer.populate_relationships()