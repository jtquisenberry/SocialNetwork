from py2neo import Graph
from py2neo import Database


class GraphResult:

    def __init__(self):
        self.graph = Graph('bolt://localhost:7687')
        self.relationships = []

    def get_entire_graph(self):

        query = 'START n=node(*) MATCH (n)-[r]->(m) RETURN n.Name as from_name,' + \
                'n.id as from_id,type(r) as relationship_type,m.Name as to_name,m.id as to_id;'

        data = self.graph.run(query)
        for d in data:
            self.relationships.append([d['from_name'], d['from_id'], d['relationship_type'], d['to_name'], d['to_id']])

        # print(self.relationships)

    def click_node(self, id):

        query = 'MATCH (n)-[r]->(m) where n.id = "{0}"  RETURN n.Name as from_name,'.format(id) + \
                'n.id as from_id,type(r) as relationship_type,m.Name as to_name,m.id as to_id;'
        data = self.graph.run(query)
        for d in data:
            self.relationships.append([d['from_name'], d['from_id'], d['relationship_type'], d['to_name'], d['to_id']])

        query = 'MATCH (n)-[r]->(m) where m.id = "{0}"  RETURN n.Name as from_name,'.format(id) + \
                'n.id as from_id,type(r) as relationship_type,m.Name as to_name,m.id as to_id;'
        data = self.graph.run(query)
        for d in data:
            self.relationships.append([d['from_name'], d['from_id'], d['relationship_type'], d['to_name'], d['to_id']])

        # print(self.relationships)

    def click_edge(self, relationship):
        query = 'MATCH (n)-[r:{0}]->(m) '.format(relationship) + \
            'RETURN n.Name as from_name,' + \
            'n.id as from_id,type(r) as relationship_type,m.Name as to_name,m.id as to_id;'

        data = self.graph.run(query)
        for d in data:
            self.relationships.append([d['from_name'], d['from_id'], d['relationship_type'], d['to_name'], d['to_id']])

        # print(self.relationships)

    def get_relationships(self):
        return self.relationships


if __name__ == '__main__':

    results = GraphResult()
    results.click_edge('ATTENDS')
    print("edge", results.get_relationships())
    results.click_node('H')
    print("node", results.get_relationships())
    results.get_entire_graph()
    print("all ", results.get_relationships())