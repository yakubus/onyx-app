// ignore_for_file: public_member_api_docs, sort_constructors_first
import 'dart:convert';

class MonthDate {
  final int month;
  final int year;

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
}
