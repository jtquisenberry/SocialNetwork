import os

from flask import Flask


def create_app(test_config=None):
    """Create and configure an instance of the Flask application."""
    app = Flask(__name__, instance_relative_config=True)

    @app.route('/hello')
    def hello():
        return 'Hello, World!'

    # register the database commands


    # apply the blueprints to the app
    #from graphserver. import graph
    import graph
    app.register_blueprint(graph.bp)

    # make url_for('index') == url_for('blog.index')
    # in another app, you might define a separate main index here with
    # app.route, while giving the blog blueprint a url_prefix, but for
    # the tutorial the blog will be the main index
    app.add_url_rule('/', endpoint='index')

    return app


if __name__ == "__main__":
    app = create_app()
    app.run(host='0.0.0.0', port=80)
