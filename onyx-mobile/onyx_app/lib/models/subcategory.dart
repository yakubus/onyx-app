// ignore_for_file: public_member_api_docs, sort_constructors_first
import 'dart:convert';

import 'package:onyx_app/models/assignement.dart';
import 'package:onyx_app/models/target.dart';

class Subcategory {
  final String id;
  final String name;
  final List<Assignement> assignement;
  final String description;
  Target? target;

  Subcategory({
    required this.id,
    required this.name,
    required this.assignement,
    required this.description,
    this.target,
  });

  Map<String, dynamic> toMap() {
    return <String, dynamic>{
      'id': id,
      'name': name,
      'assignement': assignement.map((x) {
        return x.toMap();
      }).toList(growable: false),
      'description': description,
      'target': target?.toMap(),
    };
  }

  factory Subcategory.fromMap(Map<String, dynamic> map) {
    return Subcategory(
      id: (map["id"] ?? '') as String,
      name: (map["name"] ?? '') as String,
      assignement: List<Assignement>.from(
        ((map['assignement'] ?? const <Assignement>[]) as List)
            .map<Assignement>((x) {
          return Assignement.fromMap(
              (x ?? Map<String, dynamic>.from({})) as Map<String, dynamic>);
        }),
      ),
      description: (map["description"] ?? '') as String,
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
