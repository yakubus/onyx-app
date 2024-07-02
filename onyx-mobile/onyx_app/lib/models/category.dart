// ignore_for_file: public_member_api_docs, sort_constructors_first
import 'dart:convert';

import 'package:onyx_app/models/subcategory.dart';

class BudgetCategory {
  final String id;
  final String name;
  final List<Subcategory> subcategories;

  const BudgetCategory({
    required this.id,
    required this.name,
    required this.subcategories,
  });

  Map<String, dynamic> toMap() {
    return <String, dynamic>{
      'id': id,
      'name': name,
      'subcategories': subcategories.map((x) {
        return x.toMap();
      }).toList(growable: false),
    };
  }

  factory BudgetCategory.fromMap(Map<String, dynamic> map) {
    return BudgetCategory(
      id: (map["id"] ?? '') as String,
      name: (map["name"] ?? '') as String,
      subcategories: List<Subcategory>.from(
        ((map['subcategories'] ?? const <Subcategory>[]) as List)
            .map<Subcategory>((x) {
          return Subcategory.fromMap(
              (x ?? Map<String, dynamic>.from({})) as Map<String, dynamic>);
        }),
      ),
    );
  }

  String toJson() => json.encode(toMap());

  factory BudgetCategory.fromJson(String source) =>
      BudgetCategory.fromMap(json.decode(source) as Map<String, dynamic>);
}
