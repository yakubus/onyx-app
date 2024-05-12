// ignore_for_file: public_member_api_docs, sort_constructors_first
import 'dart:convert';

class Money {
  final String currency;
  final double amount;

  Money({
    required this.currency,
    required this.amount,
  });

  Map<String, dynamic> toMap() {
    return <String, dynamic>{
      'currency': currency,
      'amount': amount,
    };
  }

  factory Money.fromMap(Map<String, dynamic> map) {
    return Money(
      currency: (map["currency"] ?? '') as String,
      amount: (map["amount"] ?? 0.0) as double,
    );
  }

  String toJson() => json.encode(toMap());

  factory Money.fromJson(String source) =>
      Money.fromMap(json.decode(source) as Map<String, dynamic>);
}
