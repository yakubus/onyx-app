// ignore_for_file: public_member_api_docs, sort_constructors_first
import 'dart:convert';

class MonthDate {
  int month;
  int year;

  MonthDate({required this.month, required this.year});

  factory MonthDate.fromMap(Map<String, dynamic> map) {
    return MonthDate(
      month: (map["month"] ?? 0) as int,
      year: (map["year"] ?? 0) as int,
    );
  }

  Map<String, dynamic> toMap() {
    return <String, dynamic>{
      'month': month,
      'year': year,
    };
  }

  String toJson() => json.encode(toMap());

  factory MonthDate.fromJson(String source) =>
      MonthDate.fromMap(json.decode(source) as Map<String, dynamic>);

  void setMonth(int month) {
    if (month > 12) {
      this.month = 12;
    } else if (month < 1) {
      this.month = 1;
    } else {
      this.month = month;
    }
  }

  void setYear(int year) {
    if (year < 2000) {
      this.year = 2000;
    } else {
      this.year = year;
    }
  }
}
