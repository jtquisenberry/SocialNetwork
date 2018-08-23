import sys
sys.path.append('../')
from parse_file import NetworkTextParser
from write_db import SocialNetworkWriter
#from .parse_file import  NetworkTextParser
#from .write_db import SocialNetworkWriter
import sys
import argparse
import os


if __name__ == '__main__':

    # Setup file inputs
    bolt_uri = 'bolt://localhost:7687'
    #entity_file = os.path.dirname(__file__) + '/data/entity_properties.txt'
    #relationship_file = os.path.dirname(__file__) + '/data/entity_relationships.txt'

    entity_file = os.path.abspath(os.path.join(os.path.dirname(__file__), 'data', 'entity_properties.txt'))
    relationship_file = os.path.abspath(os.path.join(os.path.dirname(__file__), 'data', 'entity_relationships.txt'))

    if len(sys.argv) >= 4:
        bolt_uri = sys.argv[1]
        entity_file = sys.argv[2]
        relationship_file = sys.argv[3]
    elif len(sys.argv) == 2:
        bolt_uri = sys.argv[1]
    elif len(sys.argv) in [3]:
        raise argparse.ArgumentParser('Please enter three command line arguments - a URI, the properties file, the relationships file.')

    # This would be a good time to implement try...except to determine whether the
    # specified files exist.

    parser = NetworkTextParser()
    parser.parse_properties(entity_file)
    parser.parse_relationships(relationship_file)

    # An example of dependency injection. If I had other parsers, such as an
    # RDMS parser, I would have both NetworkTextParser and NetworkRDMSParser
    # implement a common interface.
    writer = SocialNetworkWriter(parser, bolt_uri)
    writer.populate_nodes()
    writer.populate_relationships()