from flask import Flask, render_template, request, redirect, jsonify, session, g
import sqlite3

app = Flask(__name__)


################################
# Product synchronous functions
################################

@app.route('/')
def home():
    args = request.args
    print(args)  # If arguments are passed with GET
    return render_template('home.html')

@app.route('/product')
def product():
    conn = sqlite3.connect('database.db')
    cursor = conn.cursor()
    cursor.execute("SELECT * FROM product")
    data = cursor.fetchall()
    conn.close()
    return render_template('product.html', products=data)

@app.route('/product_sync')
def product_sync():
    conn = sqlite3.connect('database.db')
    cursor = conn.cursor()
    cursor.execute("SELECT * FROM product")
    products = cursor.fetchall()
    conn.close()
    return render_template('product_sync.html', products=products)

@app.route('/update/<int:id>', methods=['GET', 'POST'])
def update(id):
    if request.method == 'POST':
        name = request.form['name']
        color = request.form['color']
        quantity = request.form['quantity']
        conn = sqlite3.connect('database.db')
        c = conn.cursor()
        c.execute('UPDATE product SET name=?, color=?, quantity=? WHERE id=?', (name, color, quantity, id))
        conn.commit()
        conn.close()
        return redirect('/product')
    else:
        conn = sqlite3.connect('database.db')
        c = conn.cursor()
        c.execute('SELECT * FROM Product WHERE id=?', (id,))
        product = c.fetchone()
        conn.close()
        return render_template('update.html', product=product)

@app.route('/delete/<int:id>')
def delete(id):
    conn = sqlite3.connect('database.db')
    c = conn.cursor()
    c.execute('DELETE FROM Product WHERE id=?', (id,))
    conn.commit()
    conn.close()
    return redirect('/product')

@app.route('/add', methods=['GET', 'POST'])
def add():
    if request.method == 'POST':
        name = request.form['name']
        color = request.form['color']
        quantity = request.form['quantity']
        conn = sqlite3.connect('database.db')
        c = conn.cursor()
        c.execute('INSERT INTO Product (name, color, quantity) VALUES (?, ?, ?)', (name, color, quantity))
        conn.commit()
        conn.close()
        return redirect('/product')
    else:
        return render_template('add.html')

@app.route('/view/<int:id>', methods=['GET'])
def view(id):
    conn = sqlite3.connect('database.db')
    cursor = conn.cursor()
    cursor.execute("SELECT * FROM product WHERE id=?", (id,))
    data = cursor.fetchone()
    conn.close()
    return render_template('view.html', product=data)


################################
# Product API
################################

@app.route('/api/product', methods=['GET'])
def get_products():
    conn = sqlite3.connect('database.db')
    cursor = conn.cursor()
    cursor.execute("SELECT * FROM product")
    data = cursor.fetchall()
    conn.close()
    return jsonify(data)

@app.route('/api/product', methods=['POST'])
def add_product():
    data = request.get_json()
    name = data['name']
    color = data['color']
    quantity = data['quantity']
    conn = sqlite3.connect('database.db')
    cursor = conn.cursor()
    cursor.execute("INSERT INTO product (name, color, quantity) VALUES (?, ?, ?)", (name, color, quantity))
    conn.commit()
    conn.close()
    return jsonify({'success': True})

@app.route('/api/product/<int:id>', methods=['GET', 'POST', 'DELETE'])
def product2(id):
    if request.method == 'GET':
        conn = sqlite3.connect('database.db')
        cursor = conn.cursor()
        cursor.execute("SELECT * FROM product WHERE id=?", (id,))
        record = cursor.fetchone()
        product = {'id':record[0], 'name':record[1], 'color':record[2], 'quantity':record[3]}
        conn.close()
        return jsonify(product)
    if request.method == 'DELETE':
        conn = sqlite3.connect('database.db')
        cursor = conn.cursor()
        result = cursor.execute("DELETE FROM product WHERE id=?", (id,))
        row_count = result.rowcount
        conn.commit()
        conn.close()
        return jsonify({'success': True})
    if request.method == 'POST':
        request_json = request.get_json()
        name = request_json['name']
        color = request_json['color']
        quantity = request_json['quantity']
        conn = sqlite3.connect('database.db')
        cursor = conn.cursor()
        result = cursor.execute('UPDATE Product SET name=?, color=?, quantity=? WHERE id=?', (name, color, quantity, id))
        row_count = result.rowcount
        conn.commit()
        conn.close()
        return redirect('/product')


################################
# Chat Synchronous Functions
################################

@app.route('/chat_sync', methods=['GET'])
def chat_sync():
    args = request.args
    prompt = args.get('prompt', '')
    response = f"{prompt}: my_response"
    return render_template('chat_sync.html', response=response)

@app.route('/chat_rest', methods=['GET', 'POST'])
def chat_rest():
    if request.method == 'GET':
        args = request.args
        prompt = args.get('prompt', '')
        response = f"{prompt}: my_response"
        return render_template('chat_rest.html', response=response)
    elif request.method == 'POST':
        a = 1
        pass


################################
# Chat API
################################

@app.route('/api/chat', methods=['POST'])
def chat_api():
    data = request.json
    prompt = data.get('message', '')
    return jsonify({'result': f"{prompt}: my_result"})


if __name__ == '__main__':
    app.app_context().push()
    print(f"app: {app}, app ID: {id(app)}, app Type: {type(app)}")
    demo_object = {"a": 1, "b": 2, "c": 3}
    app.config['demo_object'] = demo_object
    app.run(debug=True)
