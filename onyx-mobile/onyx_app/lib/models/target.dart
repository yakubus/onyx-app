// ignore_for_file: public_member_api_docs, sort_constructors_first
import 'dart:convert';

import 'package:onyx_app/models/money.dart';
import 'package:onyx_app/models/month_date.dart';

class Target {
  final MonthDate upToMonth;
  final MonthDate startedAt;
  final Money targetAmount;
  final Money collectedAmount;
  final Money ammountAssignedEveryMonth;

  const Target({
    required this.upToMonth,
    required this.startedAt,
    required this.targetAmount,
    required this.collectedAmount,
    required this.ammountAssignedEveryMonth,
  });

  Map<String, dynamic> toMap() {
    return <String, dynamic>{
      'upToMonth': upToMonth.toMap(),
      'startedAt': startedAt.toMap(),
      'targetAmount': targetAmount.toMap(),
      'collectedAmount': collectedAmount.toMap(),
      'ammountAssignedEveryMonth': ammountAssignedEveryMonth.toMap(),
    };
  }

  factory Target.fromMap(Map<String, dynamic> map) {
    return Target(
      upToMonth: MonthDate.fromMap((map["upToMonth"] ??
          Map<String, dynamic>.from({})) as Map<String, dynamic>),
      startedAt: MonthDate.fromMap((map["startedAt"] ??
          Map<String, dynamic>.from({})) as Map<String, dynamic>),
      targetAmount: Money.fromMap((map["targetAmount"] ??
          Map<String, dynamic>.from({})) as Map<String, dynamic>),
      collectedAmount: Money.fromMap((map["collectedAmount"] ??
          Map<String, dynamic>.from({})) as Map<String, dynamic>),
      ammountAssignedEveryMonth: Money.fromMap(
          (map["ammountAssignedEveryMonth"] ?? Map<String, dynamic>.from({}))
              as Map<String, dynamic>),
    );
  }

  String toJson() => json.encode(toMap());

  factory Target.fromJson(String source) =>
      Target.fromMap(json.decode(source) as Map<String, dynamic>);
}
