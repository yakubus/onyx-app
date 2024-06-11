// ignore_for_file: public_member_api_docs, sort_constructors_first
import 'dart:convert';

class ErrorOnyx {
  final String code;
  final String message;

  const ErrorOnyx({
    required this.code,
    required this.message,
  });

  factory ErrorOnyx.fromJson(Map<String, dynamic> json) {
    return ErrorOnyx(
      code: json['code'],
      message: json['message'],
    );
  }

  Map<String, dynamic> toMap() {
    return <String, dynamic>{
      'code': code,
      'message': message,
    };
  }

  factory ErrorOnyx.fromMap(Map<String, dynamic> map) {
    return ErrorOnyx(
      code: (map["code"] ?? '') as String,
      message: (map["message"] ?? '') as String,
    );
  }

  String toJson() => json.encode(toMap());
}
