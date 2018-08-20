import sys
sys.path.append('../')
from flask import (
    Blueprint, flash, g, redirect, render_template, request, url_for, jsonify
)
from werkzeug.exceptions import abort
from query_db import GraphResult



bp = Blueprint('graph', __name__)


@bp.route('/')
def index():
    """Show all the posts, most recent first."""
    '''
    db = get_db()
    posts = db.execute(
        'SELECT p.id, title, body, created, author_id, username'
        ' FROM post p JOIN user u ON p.author_id = u.id'
        ' ORDER BY created DESC'
    ).fetchall()
    '''
    print('graph.index')
    return render_template('graph.html')


@bp.route('/graph2', methods=('GET', 'POST'))
def graph2():
    print('graph.graph2')
    return render_template('graph.html')


@bp.route('/updategraph', methods=('GET', 'POST'))
def updategraph():

    clickType = request.args.get('clickType')
    entityID = request.args.get('entityID')

    print(clickType)
    print(entityID)


    print('graph.updategraph')


    result = GraphResult()

    if clickType == 'node':
        result.click_node(entityID)
    elif clickType == 'edge':
        entityID = entityID[entityID.index('*')+1:]
        result.click_edge(entityID)
    else:
        result.get_entire_graph()





    relationships = result.get_relationships()

    nodes = list()
    edges = list()
    enrolled_ids = set()

    for relationship in relationships:

        #Nodes part 1 - the from node
        if relationship[1] not in enrolled_ids:
            # Add a node only if it has not already been added.
            enrolled_ids.add(relationship[1])

            data = dict()
            data['id'] = relationship[1]
            data['label'] = relationship[0]
            data['group'] = 1
            style = dict()
            style['width'] = '20'
            style['height'] = '20'
            style['font-size'] = '8px'
            datastyle = dict()
            datastyle['data'] = data
            datastyle['style'] = style
            nodes.append(datastyle)

        # Nodes part 2 - the to node
        if relationship[4 not in enrolled_ids]:
            # Add a node only if it has not already been added.
            enrolled_ids.add(relationship[4])

            data = dict()
            data['id'] = relationship[1]
            data['label'] = relationship[0]
            data['group'] = 1
            style = dict()
            style['width'] = '20'
            style['height'] = '20'
            style['font-size'] = '8px'
            datastyle = dict()
            datastyle['data'] = data
            datastyle['style'] = style
            nodes.append(datastyle)

        # CYPHER queries were designed so that relationships
        # would be in from->to order.
        # The CYPHER queries are also responsible for ensuring that there
        # are no repeated relationships.
        data = dict()
        data['id'] = relationship[1] + "-" + relationship[4] + "*" + relationship[2]
        data['label'] = relationship[2]
        data['source'] = relationship[1]
        data['target'] = relationship[4]
        data['group'] = 1
        datastyle = dict()
        datastyle['data'] = data
        edges.append(datastyle)

    elements = dict()
    elements['nodes'] = nodes
    elements['edges'] = edges
    cytoscapeOptions = dict()
    cytoscapeOptions['elements'] = elements
    return_object = dict()
    return_object['cytoscapeOptions'] = cytoscapeOptions

    #print(return_object)
    print(jsonify(return_object))
    return jsonify(return_object)


