1. Import the dtsel’s JavaScript and CSS files into the document.

<link rel="stylesheet" href="dtsel.css" />
<script src="dtsel.js"></script>

2. Attach a basic date picker to the input field you specify.

<input name="dateTimePicker" />
instance = new dtsel.DTS('input[name="dateTimePicker"]');

3. Show the time selection sliders under the date picker.

instance = new dtsel.DTS('input[name="dateTimePicker"]',  {
  showTime: true
});

4. Hide the date picker.

instance = new dtsel.DTS('input[name="dateTimePicker"]',  {
  showTime: true,
  showDate: false
});

5. Customize the date and time formats.

instance = new dtsel.DTS('input[name="dateTimePicker"]',  {
  dateFormat: "yyyy-mm-dd",
  timeFormat: "HH:MM:SS"
});

6. Determine the direction of the date picker. Default: “top”.

instance = new dtsel.DTS('input[name="dateTimePicker"]',  {
  direction: 'BOTTOM'
});

7. Customize the calendar view:

Days (Default)
Months
Years
instance = new dtsel.DTS('input[name="dateTimePicker"]',  {
  defaultView: "MONTHS"
});

8. Specify the padding of the date picker.

instance = new dtsel.DTS('input[name="dateTimePicker"]',  {
  paddingX: 5,
  paddingY: 5
});