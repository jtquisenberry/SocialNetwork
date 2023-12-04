# Social Network with Python Flask and Bootstrap 5

# Conversion of buttons with dropdown to Bootstrap 5

* Replace `data-toggle` with `data-bs-toggle`
* Add `class="dropdown-item" to each item in the dropdown.`

```html
<div class="dropdown" style="display:inline-block;">
    <button class="btn btn-primary dropdown-toggle" type="button" id="depthMenu" data-bs-toggle="dropdown"
            aria-expanded="false">
        Graph Depth
        <span class="caret"></span>
    </button>
    <ul class="dropdown-menu" aria-labelledby="dropdownMenu1">
        <li><a class="dropdown-item" href="#" onclick="setDepth(1.0)">One Level</a></li>
        <li><a class="dropdown-item" href="#" onclick="setDepth(1.5)">One Level with Interconnections</a></li>
        <li><a class="dropdown-item" href="#" onclick="setDepth(2.0)">Two Levels</a></li>
    </ul>
</div>
```