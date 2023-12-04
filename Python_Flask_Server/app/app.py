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
    scopeResult = {
        'Pairs': [
            {'SenderEntityID': 33333, 'RecipientEntityID': 44444, 'SenderDisplay': 'aaaaa', 'RecipientDisplay': 'bbbbb',
             'Count': 42}],
        'Senders': [{'SenderEntityID': 33333, 'SenderDisplay': 'aaaaa', 'Count': 42}],
        'Recipients': [{'SenderEntityID': 44444, 'SenderDisplay': 'bbbbb', 'Count': 77}]
    }

    result = {
        'scopeResult': scopeResult,
        'cytoscapeOptions': cytoscapeOptions
    }

    return jsonify(result)

@app.route('/getGraphGeneration15')
def getGraphGeneration15():
    scopeResult = {
        'Pairs': [
            {'SenderEntityID': 33333, 'RecipientEntityID': 44444, 'SenderDisplay': 'aaaaa', 'RecipientDisplay': 'bbbbb',
             'Count': 42}],
        'Senders': [{'SenderEntityID': 33333, 'SenderDisplay': 'aaaaa', 'Count': 42}],
        'Recipients': [{'SenderEntityID': 44444, 'SenderDisplay': 'bbbbb', 'Count': 77}]
    }

    result = {
        'scopeResult': scopeResult,
        'cytoscapeOptions': cytoscapeOptions
    }

    return jsonify(result)

@app.route('/getGraphGeneration20')
def getGraphGeneration20():
    scopeResult = {
        'Pairs': [
            {'SenderEntityID': 33333, 'RecipientEntityID': 44444, 'SenderDisplay': 'aaaaa', 'RecipientDisplay': 'bbbbb',
             'Count': 42}],
        'Senders': [{'SenderEntityID': 33333, 'SenderDisplay': 'aaaaa', 'Count': 42}],
        'Recipients': [{'SenderEntityID': 44444, 'SenderDisplay': 'bbbbb', 'Count': 77}]
    }

    result = {
        'scopeResult': scopeResult,
        'cytoscapeOptions': cytoscapeOptions
    }

    return jsonify(result)

@app.route('/getGraphGeneration10Case')
def getGraphGeneration10Case():

    args = request.args
    print(args)

    scopeResult = {
        'Pairs': [{'SenderEntityID': 33333, 'RecipientEntityID': 44444, 'SenderDisplay': 'aaaaa', 'RecipientDisplay': 'bbbbb', 'Count': 42}],
        'Senders': [{'SenderEntityID': 33333, 'SenderDisplay': 'aaaaa', 'Count': 42}],
        'Recipients': [{'SenderEntityID': 44444, 'SenderDisplay': 'bbbbb', 'Count': 77}]
    }

    result = {
        'scopeResult': scopeResult,
        'cytoscapeOptions': cytoscapeOptions
    }

    return jsonify(result)



@app.route('/getGraphGeneration15Case')
def getGraphGeneration15Case():
    scopeResult = {
        'Pairs': [
            {'SenderEntityID': 33333, 'RecipientEntityID': 44444, 'SenderDisplay': 'aaaaa', 'RecipientDisplay': 'bbbbb',
             'Count': 42}],
        'Senders': [{'SenderEntityID': 33333, 'SenderDisplay': 'aaaaa', 'Count': 42}],
        'Recipients': [{'SenderEntityID': 44444, 'SenderDisplay': 'bbbbb', 'Count': 77}]
    }

    result = {
        'scopeResult': scopeResult,
        'cytoscapeOptions': cytoscapeOptions
    }

    return jsonify(result)

@app.route('/getGraphGeneration20Case')
def getGraphGeneration20Case():
    scopeResult = {
        'Pairs': [
            {'SenderEntityID': 33333, 'RecipientEntityID': 44444, 'SenderDisplay': 'aaaaa', 'RecipientDisplay': 'bbbbb',
             'Count': 42}],
        'Senders': [{'SenderEntityID': 33333, 'SenderDisplay': 'aaaaa', 'Count': 42}],
        'Recipients': [{'SenderEntityID': 44444, 'SenderDisplay': 'bbbbb', 'Count': 77}]
    }

    result = {
        'scopeResult': scopeResult,
        'cytoscapeOptions': cytoscapeOptions
    }

    return jsonify(result)

@app.route('/getGraphGeneration10Filter')
def getGraphGeneration10Filter():
    scopeResult = {
        'Pairs': [
            {'SenderEntityID': 33333, 'RecipientEntityID': 44444, 'SenderDisplay': 'aaaaa', 'RecipientDisplay': 'bbbbb',
             'Count': 42}],
        'Senders': [{'SenderEntityID': 33333, 'SenderDisplay': 'aaaaa', 'Count': 42}],
        'Recipients': [{'SenderEntityID': 44444, 'SenderDisplay': 'bbbbb', 'Count': 77}]
    }

    result = {
        'scopeResult': scopeResult,
        'cytoscapeOptions': cytoscapeOptions
    }

    return jsonify(result)

@app.route('/getGraphGeneration15Filter')
def getGraphGeneration15Filter():
    scopeResult = {
        'Pairs': [
            {'SenderEntityID': 33333, 'RecipientEntityID': 44444, 'SenderDisplay': 'aaaaa', 'RecipientDisplay': 'bbbbb',
             'Count': 42}],
        'Senders': [{'SenderEntityID': 33333, 'SenderDisplay': 'aaaaa', 'Count': 42}],
        'Recipients': [{'SenderEntityID': 44444, 'SenderDisplay': 'bbbbb', 'Count': 77}]
    }

    result = {
        'scopeResult': scopeResult,
        'cytoscapeOptions': cytoscapeOptions
    }

    return jsonify(result)

@app.route('/getGraphGeneration20Filter')
def getGraphGeneration20Filter():
    scopeResult = {
        'Pairs': [
            {'SenderEntityID': 33333, 'RecipientEntityID': 44444, 'SenderDisplay': 'aaaaa', 'RecipientDisplay': 'bbbbb',
             'Count': 42}],
        'Senders': [{'SenderEntityID': 33333, 'SenderDisplay': 'aaaaa', 'Count': 42}],
        'Recipients': [{'SenderEntityID': 44444, 'SenderDisplay': 'bbbbb', 'Count': 77}]
    }

    result = {
        'scopeResult': scopeResult,
        'cytoscapeOptions': cytoscapeOptions
    }

    return jsonify(result)

@app.route('/edgeClicked1')
def edgeClicked1():
    return jsonify({})

@app.route('/getSavedSearches')
def getSavedSearches():
    return jsonify([{'ArtifactID': 0, 'TextIdentifier': 'Search 1'}, {'ArtifactID': 1, 'TextIdentifier': 'Search 2'}])

if __name__ == '__main__':
    app.app_context().push()
    print(f"app: {app}, app ID: {id(app)}, app Type: {type(app)}")
    demo_object = {"a": 1, "b": 2, "c": 3}
    app.config['demo_object'] = demo_object
    app.run(debug=True)
