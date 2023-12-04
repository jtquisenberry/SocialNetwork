
var cytoscapeOptions = {

  container: document.getElementById('cy'), // container to render in

  elements: {
    nodes: [
			  { data: {"id":1,"label":"alpha","group":1}, style:{'width':'20', 'height':'20', 'font-size':'8px'}}
			, { data: {"id":2,"label":"bravo","group":2}}
		],
			
	edges: [ 
			  {data: {'id': 'e1', 'source':1,'target':2, 'group':'1'}}
			, {data: {'id': 'e2', 'source':2,'target':1, 'group':'2'}}
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
				'label':'data(id)',
				'font-size':'8px',
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

var scopeResult = {
    layout: {
        'name': 'concentric',
	    'minNodeSpacing': 10,
	    'avoidOverlap': true
  }};

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











window.onload = function () {

    if (!(typeof MyAppUrlSettings === "undefined")) {

        MyAppUrlSettings.RootCommunicator = -1;
        MyAppUrlSettings.senderEntityID = -1;
        MyAppUrlSettings.recipientEntityID = -1;

        updateCytoscapeDataMain();

    }

    setupTable('senders'); // Draw the table with top senders.
    getSavedSearches();
};











var setupGraph = function()
{
	cytoscapeOptions.container = document.getElementById('cy');
	cy = cytoscape(cytoscapeOptions);
	
	//this.q = 999;
	//this.cy2 = cy;
	//console.log(this);
	
	cy.on('tap', 'node', function (evt) {

	    var entityID = evt.cyTarget.id();
	    console.log(entityID);
	    $("#status").text("Clicked " + entityID);
	    setSender(entityID);
        
    }); 
	
	cy.on('tap', 'edge', function (evt) {
        console.log(evt.cyTarget.id());
		 
        $("#status").text("Clicked " + evt.cyTarget.id());

        edgeClicked1(evt.cyTarget.id());
		
    }); 
	
};

function updateCytoscapeDataMain()
{
    var depth = MyAppUrlSettings.Depth;
    var maximumParticipants = MyAppUrlSettings.maximumParticipants;
    var savedSearch = MyAppUrlSettings.savedSearch;
    var senderEntityID = MyAppUrlSettings.senderEntityID;
    var recipientEntityID = MyAppUrlSettings.recipientEntityID;


    // Use pre-rendered Everything

    if ((depth == 1.0) && (maximumParticipants >= 2000000000) && (savedSearch == -1) && (senderEntityID == -1) && (recipientEntityID == -1)) {
        updateCytoscapeDataGeneration10Case();
    }
    else if ((depth == 1.5) && (maximumParticipants >= 2000000000) && (savedSearch == -1) && (senderEntityID == -1) && (recipientEntityID == -1)) {
        updateCytoscapeDataGeneration15Case();
    }
    else if ((depth == 2.0) && (maximumParticipants >= 2000000000) && (savedSearch == -1) && (senderEntityID == -1) && (recipientEntityID == -1)) {
        updateCytoscapeDataGeneration20Case(2);
    }




    // Use pre-rendered SN_Pairs.
    else if ((depth == 1.0) && (maximumParticipants >= 2000000000) && (savedSearch == -1) && (senderEntityID == -1 || recipientEntityID == -1)) {
        updateCytoscapeDataGeneration10();
    }
    else if ((depth == 1.5) && (maximumParticipants >= 2000000000) && (savedSearch == -1) && (senderEntityID == -1 || recipientEntityID == -1)) {
        updateCytoscapeDataGeneration15();
    }
    else if ((depth == 2.0) && (maximumParticipants >= 2000000000) && (savedSearch == -1) && (senderEntityID == -1 || recipientEntityID == -1)) {
        updateCytoscapeDataGeneration20(2);
    }



        // Must calculate #SN_Pairs

    else if ((depth == 1.0) && ((maximumParticipants < 2000000000) || (savedSearch != -1) || (senderEntityID > -1 && recipientEntityID > -1))) {
        updateCytoscapeDataGeneration10Filter(MyAppUrlSettings.RootCommunicator, maximumParticipants, savedSearch);
    }
    else if ((depth == 1.5) && ((maximumParticipants < 2000000000) || (savedSearch != -1) || (senderEntityID > -1 && recipientEntityID > -1))) {
        updateCytoscapeDataGeneration15Filter(MyAppUrlSettings.RootCommunicator, maximumParticipants, savedSearch);
    }
    else if ((depth == 2.0) && ((maximumParticipants < 2000000000) || (savedSearch != -1) || (senderEntityID > -1 && recipientEntityID > -1))) {
        // Depth, EntityID, Maximum Participants
        updateCytoscapeDataGeneration20Filter(2, MyAppUrlSettings.RootCommunicator, maximumParticipants, savedSearch);
    }
}



function updateCytoscapeDataGeneration10() {
    $.ajax({
        url: MyAppUrlSettings.GetGraphGeneration10,
        data: {
            'SenderEntityID': MyAppUrlSettings.senderEntityID,
            'RecipientEntityID': MyAppUrlSettings.recipientEntityID
        },
        type: 'GET',
        success: function (data) {
            console.log(data);

            // global scopeResult object.
            // scopeResult contains Senders, Recipients, and Pairs.
            scopeResult = data.scopeResult;

            if (MyAppUrlSettings.currentTableType == "sender") {
                populateSenderTable2(data.scopeResult);
            }
            else if (MyAppUrlSettings.currentTableType == "recipient") {
                populateRecipientTable2(data.scopeResult);
            }
            else if (MyAppUrlSettings.currentTableType == "pair") {
                populatePairTable2(data.scopeResult);
            }


            if (data.cytoscapeOptions.elements.nodes.length > 0) {
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

function updateCytoscapeDataGeneration15() {
    $.ajax({
        url: MyAppUrlSettings.GetGraphGeneration15,
        data: {
            'SenderEntityID': MyAppUrlSettings.senderEntityID,
            'RecipientEntityID': MyAppUrlSettings.recipientEntityID
        },
        type: 'GET',
        success: function (data) {

            console.log(data);

            // global scopeResult object.
            // scopeResult contains Senders, Recipients, and Pairs.
            scopeResult = data.scopeResult;

            if (MyAppUrlSettings.currentTableType == "sender") {
                populateSenderTable2(data.scopeResult);
            }
            else if (MyAppUrlSettings.currentTableType == "recipient") {
                populateRecipientTable2(data.scopeResult);
            }
            else if (MyAppUrlSettings.currentTableType == "pair") {
                populatePairTable2(data.scopeResult);
            }


            if (data.cytoscapeOptions.elements.nodes.length > 0) {
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

function updateCytoscapeDataGeneration20(Levels) {
    $.ajax({
        url: MyAppUrlSettings.GetGraphGeneration20,
        data: {
            'Levels': Levels,
            'SenderEntityID': MyAppUrlSettings.senderEntityID,
            'RecipientEntityID': MyAppUrlSettings.recipientEntityID
        },
        type: 'GET',
        success: function (data) {
            console.log(data);
            // global scopeResult object.
            // scopeResult contains Senders, Recipients, and Pairs.
            scopeResult = data.scopeResult;

            if (MyAppUrlSettings.currentTableType == "sender") {
                populateSenderTable2(data.scopeResult);
            }
            else if (MyAppUrlSettings.currentTableType == "recipient") {
                populateRecipientTable2(data.scopeResult);
            }
            else if (MyAppUrlSettings.currentTableType == "pair") {
                populatePairTable2(data.scopeResult);
            }


            if (data.cytoscapeOptions.elements.nodes.length > 0) {
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



function updateCytoscapeDataGeneration10Case() {
    $.ajax({
        url: MyAppUrlSettings.GetGraphGeneration10Case,
        data: {
            'SenderEntityID': MyAppUrlSettings.senderEntityID,
            'RecipientEntityID': MyAppUrlSettings.recipientEntityID
        },
        type: 'GET',
        success: function (data) {
            console.log(data);

            // global scopeResult object.
            // scopeResult contains Senders, Recipients, and Pairs.
            scopeResult = data.scopeResult;

            if (MyAppUrlSettings.currentTableType == "sender") {
                populateSenderTable2(data.scopeResult);
            }
            else if (MyAppUrlSettings.currentTableType == "recipient") {
                populateRecipientTable2(data.scopeResult);
            }
            else if (MyAppUrlSettings.currentTableType == "pair") {
                populatePairTable2(data.scopeResult);
            }


            if (data.cytoscapeOptions.elements.nodes.length > 0) {
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

function updateCytoscapeDataGeneration15Case() {
    $.ajax({
        url: MyAppUrlSettings.GetGraphGeneration15Case,
        data: {
            'SenderEntityID': MyAppUrlSettings.senderEntityID,
            'RecipientEntityID': MyAppUrlSettings.recipientEntityID
        },
        type: 'GET',
        success: function (data) {

            console.log(data);

            // global scopeResult object.
            // scopeResult contains Senders, Recipients, and Pairs.
            scopeResult = data.scopeResult;

            if (MyAppUrlSettings.currentTableType == "sender") {
                populateSenderTable2(data.scopeResult);
            }
            else if (MyAppUrlSettings.currentTableType == "recipient") {
                populateRecipientTable2(data.scopeResult);
            }
            else if (MyAppUrlSettings.currentTableType == "pair") {
                populatePairTable2(data.scopeResult);
            }


            if (data.cytoscapeOptions.elements.nodes.length > 0) {
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

function updateCytoscapeDataGeneration20Case(Levels) {
    $.ajax({
        url: MyAppUrlSettings.GetGraphGeneration20Case,
        data: {
            'Levels': Levels,
            'SenderEntityID': MyAppUrlSettings.senderEntityID,
            'RecipientEntityID': MyAppUrlSettings.recipientEntityID
        },
        type: 'GET',
        success: function (data) {
            console.log(data);
            // global scopeResult object.
            // scopeResult contains Senders, Recipients, and Pairs.
            scopeResult = data.scopeResult;

            if (MyAppUrlSettings.currentTableType == "sender") {
                populateSenderTable2(data.scopeResult);
            }
            else if (MyAppUrlSettings.currentTableType == "recipient") {
                populateRecipientTable2(data.scopeResult);
            }
            else if (MyAppUrlSettings.currentTableType == "pair") {
                populatePairTable2(data.scopeResult);
            }


            if (data.cytoscapeOptions.elements.nodes.length > 0) {
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













function updateCytoscapeDataGeneration10Filter(EntityID) {
    
    MyAppUrlSettings.senderEntityID = EntityID;


    $.ajax({
        url: MyAppUrlSettings.GetGraphGeneration10Filter,
        data: {
            'SenderEntityID': MyAppUrlSettings.senderEntityID,
            'RecipientEntityID': MyAppUrlSettings.recipientEntityID,
            'MaximumParticipants': MyAppUrlSettings.maximumParticipants,
            'SavedSearch': MyAppUrlSettings.savedSearch
        },
        type: 'GET',
        success: function (data) {
            
            console.log(data);

            // global scopeResult object.
            // scopeResult contaisn Senders, Recipients, and Pairs.
            scopeResult = data.scopeResult;

            if (MyAppUrlSettings.currentTableType == "sender") {
                populateSenderTable2(data.scopeResult);
            }
            else if (MyAppUrlSettings.currentTableType == "recipient") {
                populateRecipientTable2(data.scopeResult);
            }
            else if (MyAppUrlSettings.currentTableType == "pair") {
                populatePairTable2(data.scopeResult);
            }


            if (data.cytoscapeOptions.elements.nodes.length > 0) {
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

function updateCytoscapeDataGeneration15Filter(EntityID) {

    MyAppUrlSettings.senderEntityID = EntityID;

    $.ajax({
        url: MyAppUrlSettings.GetGraphGeneration15Filter,
        data: {
            'SenderEntityID': MyAppUrlSettings.senderEntityID,
            'RecipientEntityID': MyAppUrlSettings.recipientEntityID,
            'MaximumParticipants': MyAppUrlSettings.maximumParticipants,
            'SavedSearch': MyAppUrlSettings.savedSearch
        },
        type: 'GET',
        success: function (data) {
            
            console.log(data);

            // global scopeResult object.
            // scopeResult contaisn Senders, Recipients, and Pairs.
            scopeResult = data.scopeResult;

            if (MyAppUrlSettings.currentTableType == "sender") {
                populateSenderTable2(data.scopeResult);
            }
            else if (MyAppUrlSettings.currentTableType == "recipient") {
                populateRecipientTable2(data.scopeResult);
            }
            else if (MyAppUrlSettings.currentTableType == "pair") {
                populatePairTable2(data.scopeResult);
            }


            if (data.cytoscapeOptions.elements.nodes.length > 0) {
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

function updateCytoscapeDataGeneration20Filter(levels, entityID) {

    MyAppUrlSettings.senderEntityID = entityID;

    $.ajax({
        url: MyAppUrlSettings.GetGraphGeneration20Filter,
        data: {
            'Levels': levels,
            'SenderEntityID': MyAppUrlSettings.senderEntityID,
            'RecipientEntityID': MyAppUrlSettings.recipientEntityID,
            'MaximumParticipants': MyAppUrlSettings.maximumParticipants,
            'SavedSearch': MyAppUrlSettings.savedSearch
        },
        type: 'GET',
        success: function (data) {

            console.log(data);

            // global scopeResult object.
            // scopeResult contaisn Senders, Recipients, and Pairs.
            scopeResult = data.scopeResult;

            if (MyAppUrlSettings.currentTableType == "sender") {
                populateSenderTable2(data.scopeResult);
            }
            else if (MyAppUrlSettings.currentTableType == "recipient") {
                populateRecipientTable2(data.scopeResult);
            }
            else if (MyAppUrlSettings.currentTableType == "pair") {
                populatePairTable2(data.scopeResult);
            }


            if (data.cytoscapeOptions.elements.nodes.length > 0) {
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






function edgeClicked1(edgeID)
{
    $.ajax({
        url: MyAppUrlSettings.EdgeClicked1URL,
        data: {
            'EdgeID': edgeID,
        },
        type: 'GET',
        success: function (data) {
            console.log(data);
            //window.open(data, '', 'width=10000, height=10000, resizable=1');


            
        },
        error: function (data) {
            console.log(data);
        }
    }).fail(function ($xhr) {
        var data = $xhr.responseJSON;
        console.log(data);
    });
}


/*
function populateSenderTable() {
    $.ajax({
        url: MyAppUrlSettings.WriteSenderTable,
        data: {

        },
        type: 'GET',
        success: function (data) {
            console.log(data);
            $.each(data, function (i, item) {
                $('<tr>').attr('id', item.SenderEntityID).append(
                    $('<td>').text(item.SenderDisplay),
                    $('<td>').text(item.Count)

                ).appendTo('#senderRecipientsTableBody');

            });

            $('tr').click(function (event) {
                //alert($(this).attr('id')); //trying to alert id of the clicked row     
                console.log($(this).attr('id'));

                setSender($(this).attr('id'));


                //updateScopeOnSender($(this).attr('id'), "sender");

            });
        },
        error: function (data) {
            console.log(data);
        }
    }).fail(function ($xhr) {
        var data = $xhr.responseJSON;
        console.log(data);
    });
}
*/

/*
function populateRecipientTable() {
    $.ajax({
        url: MyAppUrlSettings.WriteRecipientTable,
        data: {

        },
        type: 'GET',
        success: function (data) {
            console.log(data);
            $.each(data, function (i, item) {
                $('<tr>').attr('id', item.RecipientEntityID).append(
                    $('<td>').text(item.RecipientDisplay),
                    $('<td>').text(item.Count)

                ).appendTo('#senderRecipientsTableBody');

            });

            $('tr').click(function (event) {
                //alert($(this).attr('id')); //trying to alert id of the clicked row     
                console.log($(this).attr('id'));
                updateScopeOnRecipient($(this).attr('id'), "recipient");

            });
        },
        error: function (data) {
            console.log(data);
        }
    }).fail(function ($xhr) {
        var data = $xhr.responseJSON;
        console.log(data);
    });
}
*/

/*
function populatePairTable() {
    $.ajax({
        url: MyAppUrlSettings.WriteSenderRecipientTable,
        data: {

        },
        type: 'GET',
        success: function (data) {
            console.log(data);
            $.each(data, function (i, item) {
                $('<tr>').attr('id', item.SenderEntityID + "-" + item.RecipientEntityID).append(
                    $('<td>').text(item.SenderDisplay),
                    $('<td>').text(item.RecipientDisplay),
                    $('<td>').text(item.Count)

                ).appendTo('#senderRecipientsTableBody');

            });

            $('tr').click(function (event) {
                  
                console.log($(this).attr('id'));
                updateScopeOnPair($(this).attr('id'), "pair");

            });
        },
        error: function (data) {
            console.log(data);
        }
    }).fail(function ($xhr) {
        var data = $xhr.responseJSON;
        console.log(data);
    });
}
*/





function setupTable(type) {

    //Clear child nodes, which are the headers.
    var header = $('#participantHeader');
    header.empty();
    
    //Clear child nodes, which are data rows.
    var body = $('#senderRecipientsTableBody');
    body.empty();

    if (type == "senders")
    {
        $("#ParticipantTableTitle").text('Top Senders');
        header.append($('<th>').text('Sender'));
        header.append($('<th>').text('Count'));

        //populateSenderTable();
    }
    else if (type == "recipients")
    {
        $("#ParticipantTableTitle").text('Top Recipients');
        header.append($('<th>').text('Recipient'));
        header.append($('<th>').text('Count'));

        //populateRecipientTable();
    }
    else if (type == "sendersAndRecipients")
    {
        $("#ParticipantTableTitle").text('Top Sender ⟺ Recipient Pairs');
        header.append($('<th>').text('Sender'));
        header.append($('<th>').text('Recipient'));
        header.append($('<th>').text('Count'));

        //populateSenderRecipientTable();
    }
}



/*
function clickTableToUpdateGraph(entityID) {






    if (MyAppUrlSettings.Depth == 1.0) {
        updateCytoscapeDataGeneration10(entityID);
    }
    else if (MyAppUrlSettings.Depth == 1.5) {
        updateCytoscapeDataGeneration15(entityID);
    }
    else if (MyAppUrlSettings.Depth = 2.0) {
        updateCytoscapeDataGeneration20(2, entityID);
    }
}
*/

/*
function updateScopeOnSender(entityID, tableType)
{

    $.ajax({
        url: MyAppUrlSettings.UpdateScopeOnSender,
        data: {
            EntityID: entityID
        },
        type: 'GET',
        success: function (data) {
            console.log(data);
            scopeResult = data;

            if (tableType == "sender")
            {
                populateSenderTable2(data);
            }
            else if (tableType = "recipient")
            {
                populateRecipientTable2(data);
            }
            else if (tableType = "pair")
            {
                populatePairTable2(data);
            }
        },
        error: function (data) {
            console.log(data);
        }
    }).fail(function ($xhr) {
        var data = $xhr.responseJSON;
        console.log(data);
    });
}
*/


/*
function updateScopeOnRecipient(entityID, tableType) {

    //entityID = entityID.substring(0,entityID.indexOf("-"));

    $.ajax({
        url: MyAppUrlSettings.UpdateScopeOnRecipient,
        data: {
            EntityID: entityID
        },
        type: 'GET',
        success: function (data) {
            console.log(data);
            scopeResult = data;

            if (tableType == "sender") {
                populateSenderTable2(data);
            }
            else if (tableType = "recipient") {
                populateRecipientTable2(data);
            }
            else if (tableType = "pair") {
                populatePairTable2(data);
            }
        },
        error: function (data) {
            console.log(data);
        }
    }).fail(function ($xhr) {
        var data = $xhr.responseJSON;
        console.log(data);
    });
}
*/


/*
function updateScopeOnPair(entityID, tableType) {

    senderEntityID = entityID.substring(0, entityID.indexOf("-"));
    recipientEntityID = entityID.substring(entityID.indexOf("-") + 1, 255);

    $.ajax({
        url: MyAppUrlSettings.UpdateScopeOnPair,
        data: {
            SenderEntityID: senderEntityID,
            RecipientEntityID: recipientEntityID
        },
        type: 'GET',
        success: function (data) {
            console.log(data);
            scopeResult = data;

            if (tableType == "sender") {
                populateSenderTable2(data);
            }
            else if (tableType == "recipient") {
                populateRecipientTable2(data);
            }
            else if (tableType == "pair") {
                populatePairTable2(data);
            }
        },
        error: function (data) {
            console.log(data);
        }
    }).fail(function ($xhr) {
        var data = $xhr.responseJSON;
        console.log(data);
    });
}
*/

/*
function updateInitialScope()
{
    $.ajax({
        url: MyAppUrlSettings.UpdateScopeOnInitial,
        data: {
            // No data means top senders in the entire case.
        },
        type: 'GET',
        success: function (data) {
            
            
            scopeResult = data;
            populateSenderTable2(scopeResult);
        },
        error: function (data) {
            console.log(data);
        }
    }).fail(function ($xhr) {
        var data = $xhr.responseJSON;
        console.log(data);
    });

}
*/








function populateSenderTable2(data) {

    MyAppUrlSettings.currentTableType = 'sender';
    setupTable("sender");

    $.each(scopeResult.Senders, function (i, item) {
        $('<tr>').attr('id', item.SenderEntityID).append(
            $('<td>').text(item.SenderDisplay),
            $('<td>').text(item.Count)

        ).appendTo('#senderRecipientsTableBody');

    });

    $('tr').click(function (event) {
        console.log($(this).attr('id'));

        var entityID = $(this).attr('id');
        setSender(entityID);

        //updateScopeOnSender(entityID, "sender");
        //setSender($(this).attr('id'));
        //clickTableToUpdateGraph($(this).attr('id'));
    });
}

function populateRecipientTable2(data) {
    
    MyAppUrlSettings.currentTableType = 'recipient';
    setupTable("recipient");

    $.each(scopeResult.Recipients, function (i, item) {
        $('<tr>').attr('id', item.RecipientEntityID).append(
            $('<td>').text(item.RecipientDisplay),
            $('<td>').text(item.Count)

        ).appendTo('#senderRecipientsTableBody');

    });

    $('tr').click(function (event) {
  
        console.log($(this).attr('id'));

        var entityID = $(this).attr('id');
        setRecipient(entityID);

        //updateScopeOnRecipient($(this).attr('id'), "recipient");
        //setRecipient($(this).attr('id'));
        //clickTableToUpdateGraph($(this).attr('id'));

    });

}

function populatePairTable2(data) {

    MyAppUrlSettings.currentTableType = 'pair';
    setupTable("pair");
    
    $.each(scopeResult.Pairs, function (i, item) {
        $('<tr>').attr('id', item.SenderEntityID + "-" + item.RecipientEntityID).append(
            $('<td>').text(item.SenderDisplay),
            $('<td>').text(item.RecipientDisplay),
            $('<td>').text(item.Count)

        ).appendTo('#senderRecipientsTableBody');

    });

    $('tr').click(function (event) {
        //updateScopeOnPair($(this).attr('id'), "pair");

        console.log($(this).attr('id'));

        var senderID = $(this).attr('id');
        senderID = senderID.substring(0, senderID.indexOf("-"));
        var recipientID = $(this).attr('id');
        recipientID = recipientID.substring(recipientID.indexOf("-") + 1, recipientID.length);





        setSenderAndRecipient(senderID, recipientID);

        //setPair(senderID);
        //setSender($(this).attr('id'));
       // clickTableToUpdateGraph($(this).attr('id'));
    });

}

function getSavedSearches()
{
    $.ajax({
        url: MyAppUrlSettings.GetSavedSearches,
        data: {
            
        },
        type: 'GET',
        success: function (data) {

            // No saved search
            $('<option>').attr('value', -1).text('** Entire Population **')
                .appendTo('#savedSearchDropdown');

            // Saved searches.
            $.each(data, function (i, item) {
                $('<option>').attr('value', item.ArtifactID).text(item.TextIdentifier)
                .appendTo('#savedSearchDropdown');
            });
        },
        error: function (data) {
            console.log(data);
        }
    }).fail(function ($xhr) {
        var data = $xhr.responseJSON;
        console.log(data);
    });

}



function setSavedSearch(savedSearch)
{
    MyAppUrlSettings.savedSearch = savedSearch;
    
    updateCytoscapeDataMain();
}

function setSender(entityID)
{
    MyAppUrlSettings.RootCommunicator = entityID;
    MyAppUrlSettings.senderEntityID = entityID;
    MyAppUrlSettings.recipientEntityID = -1;
    
    updateCytoscapeDataMain();
}

function setRecipient(entityID) {
    MyAppUrlSettings.RootCommunicator = entityID;
    MyAppUrlSettings.senderEntityID = -1;
    MyAppUrlSettings.recipientEntityID = entityID;

    updateCytoscapeDataMain();
}

function setSenderAndRecipient(senderEntityID, recipientEntityID)
{
    MyAppUrlSettings.RootCommunicator = senderEntityID;
    MyAppUrlSettings.senderEntityID = senderEntityID;
    MyAppUrlSettings.recipientEntityID = recipientEntityID;

    updateCytoscapeDataMain();
}

function setDepth(depth) {

    MyAppUrlSettings.Depth = depth;

    updateCytoscapeDataMain();
}

function setMaximumParticipants(participantCount) {
    MyAppUrlSettings.maximumParticipants = participantCount;

    updateCytoscapeDataMain();
}


