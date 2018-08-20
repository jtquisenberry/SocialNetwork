# Access the entity parsing module
import sys
sys.path.append('../')
from parse_file import NetworkTextParser
from py2neo import Graph
from py2neo import Node
from py2neo import Relationship



class SocialNetworkWriter():

    def __init__(self, network):

        # Acquire the data required to build the graph
        self.properties_dictionary = network.get_properties_dictionary()
        self.entities = network.get_distinct_entities()
        self.entity_types = network.get_entity_types()
        self.relationships = network.get_relationships()

        #Setup the graph
        # Unnecessary if using Graph
        #db = Database('bolt://localhost:7687')

        #graph = Graph('bolt://localhost:7687')
        self.graph = Graph()
        self.graph.delete_all()

        #print('graph name:', graph.name)
        #print('database name', graph.database.name)

        # Setup a dictionary that stores nodes that will later be used
        # to build relationships.
        self.entity_nodes = dict()

    def populate_nodes(self):

        for entity in self.entities:

            node = Node()
            node.add_label(entity)
            node['id'] = entity

            type = self.entity_types[entity]
            node.add_label(type)

            for property in self.properties_dictionary[entity]:
                key = list(property.keys())[0]
                value = property[key]
                node[key] = value
            #print('node',node)

            self.entity_nodes[entity] = node
            self.graph.create(node)

        #print(self.entity_nodes)

    def populate_relationships(self):

        for relationship in self.relationships:
            key1 = relationship[1]
            key2 = relationship[3]

            # Lookup the node associated with a given ID
            node1 = self.entity_nodes[key1]
            node2 = self.entity_nodes[key2]

            # I could choose to strip the underscore using str.replace()
            type = relationship[0]

            # Add the relationship to the graph
            self.graph.create(Relationship(node1, type, node2))

            # Certain relationships, such as friendship, imply bidirectionality
            if type in ['FRIENDS_WITH', 'STUDIES_WITH']:
                self.graph.create(Relationship(node2, type, node1))


if __name__ == '__main__':

    entity_file = 'data/entity_properties.txt'
    relationship_file = 'data/entity_relationships.txt'
    parser = NetworkTextParser()
    parser.parse_properties(entity_file)
    parser.parse_relationships(relationship_file)

    writer = SocialNetworkWriter(parser)
    writer.populate_nodes()
    writer.populate_relationships()