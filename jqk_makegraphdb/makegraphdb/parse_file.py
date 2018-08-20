import csv


class NetworkTextParser:

    def __init__(self):
        # About private attributes:
        # I could use an underscore a dunderscore to make these attributes
        # private, but it is more "Pythonic" to make them public. Additionally,
        # there is always a way to gain access to these properties.

        # Contains a list of distinct entities and their types.
        # To accommodate for the edge case where there is a property
        # but not a type, such an entity is assigned type = "Unspecified".
        self.distinct_entities = set()

        self.entity_types = dict()

        self.properties_dict = dict()

        self.relationships = []

    def parse_properties(self, filename='data/entity_properties.txt'):
        with open(filename) as csv_file:
            csv_reader = csv.reader(csv_file, delimiter='\t')
            line_count = 0

            properties = []

            for row in csv_reader:
                if line_count == 0:
                    #print(f'Column names are {", ".join(row)}')
                    line_count += 1
                else:
                    #print(f'\t{row[0]} works in the {row[1]} department, and was born in {row[2]}.')
                    if len(row) == 3:
                        id = row[0].strip()
                        property = row[1].strip()
                        value = row[2].strip()

                        if id not in self.properties_dict:
                            self.properties_dict[id] = [{property:value}]
                        else:
                            # print(self.properties_dict)
                            # print('current id', id)
                            # print('current value', self.properties_dict[id])
                            self.properties_dict[id].append({property:value})




                        self.distinct_entities.add(id)
                        self.entity_types[id] = "Unspecified"

                        properties.append([id,property,value])

                    line_count += 1
            #print(f'Processed {line_count} lines.')
            return properties

    def parse_relationships(self, filename='data/entity_relationships.txt'):
        with open(filename) as csv_file:
            csv_reader = csv.reader(csv_file, delimiter='\t')
            line_count = 0

            for row in csv_reader:
                if line_count == 0:
                    #print(f'Column names are {", ".join(row)}')
                    line_count += 1
                else:
                    if len(row) == 5:
                        relationship = row[0].strip()
                        id1 = row[1].strip()
                        type1 = row[2].strip()
                        id2 = row[3].strip()
                        type2 = row[4].strip()

                        self.distinct_entities.add(id1)
                        self.distinct_entities.add(id2)

                        # This logic assumes that an entity can have at most one type.
                        # To implement multiple types, I would use the list of dictionaries
                        # technique I used to store properties.
                        self.entity_types[id1] = type1
                        self.entity_types[id2] = type2

                        self.relationships.append([relationship,id1,type1,id2,type2])

                    line_count += 1
            #print(f'Processed {line_count} lines.')
            #rint(relationships)
            #return self.relationships


    def get_distinct_entities(self):
        return self.distinct_entities

    def get_properties_dictionary(self):
        return self.properties_dict

    def get_entity_types(self):
        return self.entity_types

    def get_relationships(self):
        return self.relationships


'''
if __name__ == '__main__':

    # Input files
    entity_properties_file = 'data/entity_properties.txt'
    entity_relationships_file = 'data/entity_relationships.txt'

    # Perform parsing
    network = NetworkTextParser()
    properties = network.parse_properties(entity_properties_file)
    network.parse_relationships(entity_relationships_file)

    properties_dictionary = network.get_properties_dictionary()
    entities = network.get_distinct_entities()
    entity_types = network.get_entity_types()
    relationships = network.get_relationships()

    print('properties', properties)
    print('properties dictionary', properties_dictionary)
    print('relationships', relationships)
    print('entities', sorted(list(entities)))
    print('entity types,', entity_types)

'''



