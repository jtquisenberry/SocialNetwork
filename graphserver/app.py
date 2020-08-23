import os
import sys
import argparse

from flask import Flask


def create_app(test_config=None, bolt_uri = ''):
    """Create and configure an instance of the Flask application."""
    app = Flask(__name__, instance_relative_config=True)

    @app.route('/hello')
    def hello():
        return 'Hello, World!'

    # register the database commands


    # apply the blueprints to the app
    #from graphserver. import graph
    import graph
    graph.bolt_uri = bolt_uri
    app.register_blueprint(graph.bp)

    # make url_for('index') == url_for('blog.index')
    # in another app, you might define a separate main index here with
    # app.route, while giving the blog blueprint a url_prefix, but for
    # the tutorial the blog will be the main index
    app.add_url_rule('/', endpoint='index')

    return app


if __name__ == "__main__":

    bolt_uri = 'bolt://localhost:7687'

    if len(sys.argv) >= 2:
        bolt_uri = sys.argv[1]


    app = create_app(bolt_uri=bolt_uri)
    # app.run(host='0.0.0.0', port=80)  # Original
    app.run(host='127.0.0.1', port=5556)  # Windows 10