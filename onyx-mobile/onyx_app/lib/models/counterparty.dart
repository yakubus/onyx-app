import 'dart:convert';

// ignore_for_file: public_member_api_docs, sort_constructors_first
class Counterparty {
  final String id;
  final String name;
  final String type;
  const Counterparty({
    required this.id,
    required this.name,
    required this.type,
  });

  Map<String, dynamic> toMap() {
    return <String, dynamic>{
      'id': id,
      'name': name,
      'type': type,
    };
  }

  factory Counterparty.fromMap(Map<String, dynamic> map) {
    return Counterparty(
      id: (map["id"] ?? '') as String,
      name: (map["name"] ?? '') as String,
      type: (map["type"] ?? '') as String,
    );
  }

  String toJson() => json.encode(toMap());

  factory Counterparty.fromJson(String source) =>
      Counterparty.fromMap(json.decode(source) as Map<String, dynamic>);
}
