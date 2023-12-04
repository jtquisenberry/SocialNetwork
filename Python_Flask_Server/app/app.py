from flask import Flask, render_template, request, redirect, jsonify, session, g
import sqlite3
from cytoscape_options import cytoscapeOptions

app = Flask(__name__)


################################
# Product synchronous functions
################################

@app.route('/')
def home():
    args = request.args
    print(args)  # If arguments are passed with GET
    return render_template('home.html')

@app.route('/contact')
def contact():
    return render_template('contact.html')

@app.route('/getGraphGeneration10')
def getGraphGeneration10():
    return jsonify(cytoscapeOptions)

@app.route('/getGraphGeneration15')
def getGraphGeneration15():
    return jsonify({})

@app.route('/getGraphGeneration20')
def getGraphGeneration20():
    return jsonify({})

@app.route('/getGraphGeneration10Case')
def getGraphGeneration10Case():
    return jsonify({})

@app.route('/getGraphGeneration15Case')
def getGraphGeneration15Case():
    return jsonify({})

@app.route('/getGraphGeneration20Case')
def getGraphGeneration20Case():
    return jsonify({})

@app.route('/getGraphGeneration10Filter')
def getGraphGeneration10Filter():
    return jsonify({})

@app.route('/getGraphGeneration15Filter')
def getGraphGeneration15Filter():
    return jsonify({})

@app.route('/getGraphGeneration20Filter')
def getGraphGeneration20Filter():
    return jsonify({})

@app.route('/edgeClicked1')
def edgeClicked1():
    return jsonify({})

@app.route('/getSavedSearches')
def getSavedSearches():
    return jsonify({})




if __name__ == '__main__':
    app.app_context().push()
    print(f"app: {app}, app ID: {id(app)}, app Type: {type(app)}")
    demo_object = {"a": 1, "b": 2, "c": 3}
    app.config['demo_object'] = demo_object
    app.run(debug=True)
