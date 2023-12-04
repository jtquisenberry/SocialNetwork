
cytoscapeOptions = {
'elements': {
    'nodes': [
        {'data': {"id": 1, "label": "cows", "group": 1}, 'style': {'width': '20', 'height': '20', 'font-size': '8px'}}
        , {'data': {"id": 2, "label": "pigs", "group": 2}}
    ],

    'edges': [
        {'data': {'id': 'e1', 'source': 1, 'target': 2, 'group': '1'}}
        , {'data': {'id': 'e2', 'source': 2, 'target': 1, 'group': '2'}}
    ]
},

'style': [
{
'selector': 'node',
'style': {
    'background-color': '#666',
    'font-size': '10px',
    'label': 'data(label)',
    'width': '25',
    'height': '25',
    'shape': 'circle',
    'text-outline-color': '#ffffff',
    'text-outline-width': '1px'
}
},

{
    'selector': 'edge',
    'style': {
               'width': 1,
'curve-style': 'bezier',
'control-point-step-size': '20',
'target-arrow-shape': 'triangle',
'label': 'data(id)',
'font-size': '8px',
'text-rotation': 'autorotate',
'text-outline-color': '#ffffff',
'text-outline-width': '1px'
}
},

{
    'selector': "node[group=1]",
    'style': {
        'background-color': '#FF0000'
    }
},

{
    'selector': "node[group=2]",
    'style': {
        'background-color': '#97C2FF'
    }
},

{
    'selector': "edge[group=\"1\"]",
    'style': {
        'line-color': '#FF0000',
        'target-arrow-color': '#FF0000'
    }
},

{
    'selector': "edge[group='2']",
    'style': {
        'line-color': '#97C2FF',
        'target-arrow-color': '#97C2FF'
    }
}
],

'layout': {
    'name': 'concentric',
    'minNodeSpacing': 10,
    'avoidOverlap': 'true'
}

}
