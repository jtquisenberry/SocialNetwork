//alert("dogs");

window.onload = function() {
	 cy = cytoscape({

  container: document.getElementById('cy'), // container to render in

  elements: {
    nodes: [
			  { data: {"id":1,"label":"Jean-Luc Picard","group":1}, style:{'width':'20', 'height':'20', 'font-size':'8px'}}
			, { data: {"id":2,"label":"William Riker","group":2}}
			, { data: {"id":3,"label":"Data","group":2}}
			, { data: {"id":4,"label":"Tasha Yar","group":2}}
			, { data: {"id":5,"label":"Beverly Crusher","group":2}}
			, { data: {"id":6,"label":"Deanna Troi","group":2}}
			, { data: {"id":9,"label":"Worf","group":2}}
			, { data: {"id":12,"label":"Geordi La Forge","group":2}}
			, { data: {"id":14,"label":"Reginald Barclay","group":2}}
			, { data: {"id":15,"label":"Wesley Crusher","group":2}}
			, { data: {"id":16,"label":"Ro Laren","group":2}}
			, { data: {"id":17,"label":"Guinan","group":2}}
			, { data: {"id":19,"label":"Q","group":2}}
		],
			
	edges: [ 
			  {data: {'id': 'e1', 'source':1,'target':2, 'group':'1'}}
			, {data: {'id': 'e2', 'source':3,'target':1, 'group':'2'}}
			, {data: {'id': 'e3', 'source':4,'target':1, 'group':'2'}}
			, {data: {'id': 'e4', 'source':5,'target':1, 'group':'2'}}
			, {data: {'id': 'e5', 'source':6,'target':1, 'group':'2'}}
			, {data: {'id': 'e6', 'source':1,'target':3, 'group':'1'}}
			, {data: {'id': 'e7', 'source':1,'target':5, 'group':'1'}}
			, {data: {'id': 'e8', 'source':9,'target':1, 'group':'2'}}
			, {data: {'id': 'e9', 'source':2,'target':1, 'group':'2'}}
			, {data: {'id': 'e10', 'source':1,'target':9, 'group':'1'}}
			, {data: {'id': 'e11', 'source':1,'target':12, 'group':'1'}}
			, {data: {'id': 'e12', 'source':1,'target':6, 'group':'1'}}
			, {data: {'id': 'e13', 'source':1,'target':14, 'group':'1'}}
			, {data: {'id': 'e14', 'source':1,'target':15, 'group':'1'}}
			, {data: {'id': 'e15', 'source':16,'target':1, 'group':'2'}}
			, {data: {'id': 'e16', 'source':17,'target':1, 'group':'2'}}
			, {data: {'id': 'e17', 'source':14,'target':1, 'group':'2'}}
			, {data: {'id': 'e18', 'source':1,'target':19, 'group':'1'}}
			, {data: {'id': 'e19', 'source':1,'target':16, 'group':'1'}}
			, {data: {'id': 'e20', 'source':15,'target':1, 'group':'2'}}
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
	'avoidOverlap': false

  }

});
	
	cy.on('tap', 'node', function (evt) {
         console.log(evt.cyTarget.id());
		 
		 $("#status").text("Clicked " + evt.cyTarget.id());
		 
		 //window.open('https://www.google.com/', 'newwindow', 'width=10000, height=10000');
    }); 
	
	cy.on('tap', 'edge', function (evt) {
         console.log(evt.cyTarget.id());
		 
		 $("#status").text("Clicked " + evt.cyTarget.id());
		 
		 //window.open('https://www.google.com/', 'newwindow', 'width=10000, height=10000');
    }); 
	
	
	console.log(cy.container);
};

var cows =  function()
{
	var int1 = 777;
	int2 = 778;
	int3: 779;
	this.qqq = 333;
	
	this.init = function(){
		
		int1 = 777;
		int2 = 888;
		int3 = 999;
	};
	
	this.init();
	
	
	
	
	
};

var cows2 = new cows();

var myObj = new MyClass(2, true);

function MyClass(v1, v2) 
{
    // ...
	var xxx = 111;
	yyy = 222;
	self = this;
	this.mmm = 555;

    // pub methods
    this.init = function() {
		xxx = 222;
		yyy = 333;
		self.zzz = 444;
        // do some stuff        
    };

    // ...
	this.init();

    this.init(); // <------------ added this
}





console.log(cows.int1);
console.log("xxx");