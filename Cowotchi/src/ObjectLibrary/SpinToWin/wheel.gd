extends Control

@onready var chart: Chart = $Chart

var f1: Function

func _ready() -> void:
	var val_arr: Array = [ 45, 45, 45, 45, 45, 45, 45, 45 ]
	var key_arr: Array = [ "Cow", "Pig", "?????", "?????", "?????", "?????", "?????", "?????" ]

	var cp: ChartProperties = ChartProperties.new()
	cp.colors.frame = Color("#161a1d")
	cp.colors.background = Color.TRANSPARENT
	cp.colors.grid = Color("#283442")
	cp.colors.ticks = Color("#283442")
	cp.colors.text = Color.WHITE_SMOKE
	cp.draw_bounding_box = false
	cp.title = ""
	cp.draw_grid_box = false

	var gradient: Gradient = Gradient.new()
	gradient.set_color(0, Color.AQUAMARINE)
	gradient.set_color(1, Color.DEEP_PINK)

	# Let's add values to our functions
	f1 = Function.new(
		val_arr, key_arr, "Language", # This will create a function with x and y values taken by the Arrays 
						# we have created previously. This function will also be named "Pressure"
						# as it contains 'pressure' values.
						# If set, the name of a function will be used both in the Legend
						# (if enabled thourgh ChartProperties) and on the Tooltip (if enabled).
		{
			gradient = gradient,
			type = Function.Type.PIE
		}
	)
	
	# Now let's plot our data
	chart.plot([f1], cp)

func _process(delta: float) -> void:
	pass
