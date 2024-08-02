import 'package:flutter/material.dart';

class MonthYearDialog extends StatefulWidget {
  final DateTime initialDate;
  final DateTime firstDate;
  final DateTime lastDate;
  final ValueChanged<DateTime> onDateChanged;

  const MonthYearDialog({
    super.key,
    required this.initialDate,
    required this.firstDate,
    required this.lastDate,
    required this.onDateChanged,
  });

  @override
  // ignore: library_private_types_in_public_api
  _MonthYearDialogState createState() => _MonthYearDialogState();
}

class _MonthYearDialogState extends State<MonthYearDialog> {
  late int selectedYear;
  late int selectedMonth;

  @override
  void initState() {
    super.initState();
    selectedYear = widget.initialDate.year;
    selectedMonth = widget.initialDate.month;
  }

  @override
  Widget build(BuildContext context) {
    return AlertDialog(
      title: const Text('Wybierz miesiąc i rok'),
      content: Column(
        mainAxisSize: MainAxisSize.min,
        children: [
          DropdownButton<int>(
            value: selectedYear,
            items: List.generate(
              widget.lastDate.year - widget.firstDate.year + 1,
              (index) => widget.firstDate.year + index,
            ).map((year) {
              return DropdownMenuItem(
                value: year,
                child: Text(year.toString()),
              );
            }).toList(),
            onChanged: (value) {
              setState(() {
                if (value != null) {
                  selectedYear = value;
                }
              });
            },
          ),
          DropdownButton<int>(
            value: selectedMonth,
            items: List.generate(12, (index) => index + 1).map((month) {
              return DropdownMenuItem(
                value: month,
                child: Text(month.toString()),
              );
            }).toList(),
            onChanged: (value) {
              setState(() {
                if (value != null) {
                  selectedMonth = value;
                }
              });
            },
          ),
        ],
      ),
      actions: [
        TextButton(
          onPressed: () {
            Navigator.of(context).pop();
          },
          child: const Text('Anuluj'),
        ),
        TextButton(
          onPressed: () {
            widget.onDateChanged(DateTime(selectedYear, selectedMonth));
            Navigator.of(context).pop();
          },
          child: const Text('Wybierz'),
        ),
      ],
    );
  }
}
