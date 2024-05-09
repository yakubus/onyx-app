// ignore_for_file: public_member_api_docs, sort_constructors_first
import 'dart:convert';

import 'package:onyx_app/models/assignement.dart';
import 'package:onyx_app/models/target.dart';

class Subcategory {
  final String id;
  final String name;
  final Assignement assignement;
  Target? target;

  Subcategory({
    required this.id,
    required this.name,
    required this.assignement,
    this.target,
  });

  Map<String, dynamic> toMap() {
    return <String, dynamic>{
      'id': id,
      'name': name,
      'assignement': assignement.toMap(),
      'target': target?.toMap(),
    };
  }

  factory Subcategory.fromMap(Map<String, dynamic> map) {
    return Subcategory(
      id: (map["id"] ?? '') as String,
      name: (map["name"] ?? '') as String,
      assignement: Assignement.fromMap((map["assignement"] ??
          Map<String, dynamic>.from({})) as Map<String, dynamic>),
      target: map['target'] != null
          ? Target.fromMap((map["target"] ?? Map<String, dynamic>.from({}))
              as Map<String, dynamic>)
          : null,
    );
  }

  String toJson() => json.encode(toMap());

  factory Subcategory.fromJson(String source) =>
      Subcategory.fromMap(json.decode(source) as Map<String, dynamic>);
}
