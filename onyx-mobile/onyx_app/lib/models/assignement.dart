// ignore_for_file: public_member_api_docs, sort_constructors_first
import 'dart:convert';

import 'package:onyx_app/models/money.dart';
import 'package:onyx_app/models/month_date.dart';

class Assignement {
  final MonthDate month;
  final Money assignedAmount;
  final Money actualAmount;

  const Assignement({
    required this.month,
    required this.assignedAmount,
    required this.actualAmount,
  });

  Map<String, dynamic> toMap() {
    return <String, dynamic>{
      'month': month.toMap(),
      'assignedAmount': assignedAmount.toMap(),
      'actualAmount': actualAmount.toMap(),
    };
  }

  factory Assignement.fromMap(Map<String, dynamic> map) {
    return Assignement(
      month: MonthDate.fromMap((map["month"] ?? Map<String, dynamic>.from({}))
          as Map<String, dynamic>),
      assignedAmount: Money.fromMap((map["assignedAmount"] ??
          Map<String, dynamic>.from({})) as Map<String, dynamic>),
      actualAmount: Money.fromMap((map["actualAmount"] ??
          Map<String, dynamic>.from({})) as Map<String, dynamic>),
    );
  }

  String toJson() => json.encode(toMap());

  factory Assignement.fromJson(String source) =>
      Assignement.fromMap(json.decode(source) as Map<String, dynamic>);
}
