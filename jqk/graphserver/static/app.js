
var cytoscapeOptions = {

  container: document.getElementById('cy'), // container to render in

  elements: {
    nodes: [
			  { data: {"id":1,"label":"alpha","group":1}, style:{'width':'20', 'height':'20', 'font-size':'8px'}}
			, { data: {"id":2,"label":"bravo","group":2}}
		],

	edges: [
			  {data: {'id': 'e1', 'label':'aaa', 'source':1,'target':2, 'group':'1'}}
			, {data: {'id': 'e2', 'label':'bbb', 'source':2,'target':1, 'group':'2'}}
		]
  },


	style: [ // the stylesheet for the graph
		{
			selector: 'node',
			style: {
				'background-color': '#666',
				'font-size': '10px',
				'label': 'data(label)',
				'width':'25',
				'height':'25',
				'shape':'circle',
				'text-outline-color':'#ffffff',
				'text-outline-width':'1px'
				//, 'text-valign':'center'
			}
		},

		{
			selector: 'edge',
			style: {
				'width': 1,
				//'line-color': '#00ff00',
				'curve-style': 'bezier',
				'control-point-step-size':'20',
				//'target-arrow-color': '#ff0000',
				'target-arrow-shape': 'triangle',
				'label':'data(label)',
				'font-size':'4px',
				'text-rotation':'autorotate',
				'text-outline-color':'#ffffff',
				'text-outline-width':'1px'
			}
		},

		{
			selector:"node[group=1]",
			style: {
				'background-color':'#FF0000'
			}
		},

		{
			selector:"node[group=2]",
			style: {
				'background-color':'#97C2FF'
			}
		},

		{
			selector:"edge[group=\"1\"]",
			style: {
				'line-color':'#FF0000',
				'target-arrow-color':'#FF0000'
			}
		},

		{
			selector:"edge[group='2']",
			style: {
				'line-color':'#97C2FF',
				'target-arrow-color':'#97C2FF'
			}
		}
	],

  layout: {
    'name': 'concentric',
	'minNodeSpacing': 10,
	'avoidOverlap': true
  }

};

var scopeResult = {};

function setCytoscapeStyle(whichLayout) {

    //whichLayout = "concentricWithOverlap";

    var layout = {};

    if (whichLayout == "concentricNoOverlap") {
        layout = {
            'name': 'concentric',
            'minNodeSpacing': 10,
            'avoidOverlap': true
        };
    }
    else if (whichLayout == "concentricWithOverlap") {
            layout = {
                'name': 'concentric',
                'minNodeSpacing': 10,
                'avoidOverlap': false
            };
    }
    else if (whichLayout == "grid") {
        layout = {
            name: 'grid'
        };
    }

    else if (whichLayout == "spread") {
        layout = {
            name: 'spread'
        };
    }

    else if (whichLayout == "cose") {
        layout = {
            name: 'cose',
            idealEdgeLength: 100,
            nodeOverlap: 20,
            refresh: 20,
            fit: true,
            padding: 30,
            randomize: false,
            componentSpacing: 100,
            nodeRepulsion: 400000,
            edgeElasticity: 100,
            nestingFactor: 5,
            gravity: 80,
            numIter: 1000,
            initialTemp: 200,
            coolingFactor: 0.95,
            minTemp: 1.0
        };

    }

    else if (whichLayout == "cose-bilkent") {
        layout = {
            name: 'cose-bilkent',
            animate: false
        };

    }

        cytoscapeOptions.layout = layout;

        setupGraph();

}


/* Resize the graph */
/* Setup event handlers */
var setupGraph = function()
{
	cytoscapeOptions.container = document.getElementById('cy');
	cy = cytoscape(cytoscapeOptions);

	cy.on('click', 'node', function (evt) {

        var entityID = this.id();
	    // console.log(evt.cyTarget.id());
	    // var entityID = evt.cyTarget.id();

	    console.log(entityID);

        // Update the browser with the ID of the clicked edge.
	    $("#status").text("Clicked " + entityID);
	    updateCytoscapeDataGeneration10("node",entityID);

    });

	cy.on('click', 'edge', function (evt) {

        var entityID = this.id();

        // Cytoscape 1.6 logic
        // console.log(evt.cyTarget.id());
        console.log(entityID);

        // Update the browser with the ID of the clicked edge.
        $("#status").text("Clicked " + entityID);
        updateCytoscapeDataGeneration10("edge",entityID);
    });

};



function updateCytoscapeDataGeneration10(clickType, entityID) {
    $.ajax({
        url: '/updategraph',
        data: {
            'clickType': clickType,
            'entityID': entityID
        },
        type: 'GET',
        success: function (data) {
            //console.log(data);

            console.log("updateCytoscapeDataGeneration10 success");

            // global scopeResult object.
            // scopeResult contains Senders, Recipients, and Pairs.
            scopeResult = data.scopeResult;

            if (data.cytoscapeOptions != null && data.cytoscapeOptions.elements.nodes.length > 0) {
                cytoscapeOptions.elements = data.cytoscapeOptions.elements;
            }
            else {
                cytoscapeOptions.elements =
                {
                    nodes: [
                              { data: { "id": 9999, "label": 'Placeholder', "group": 1 }, style: { 'width': '20', 'height': '20', 'font-size': '8px' } }
                    ],
                    edges: [
                    ]
                }
            }

            setupGraph();

        },
        error: function (data) {
            console.log(data);
        }
    }).fail(function ($xhr) {
        var data = $xhr.responseJSON;
        console.log(data);
    });
}

