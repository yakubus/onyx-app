class UserServiceModel {
  final bool isSuccess;
  final bool isFailure;
  final Error error;
  final UserData value;

  UserServiceModel(
      {required this.isSuccess,
      required this.isFailure,
      required this.error,
      required this.value});

  factory UserServiceModel.fromJson(Map<String, dynamic> json) {
    return UserServiceModel(
      isSuccess: json['isSuccess'],
      isFailure: json['isFailure'],
      error: Error.fromJson(json['error']),
      value: UserData.fromJson(json['value']),
    );
  }
}

class Error {
  final String code;
  final String message;

  Error({required this.code, required this.message});

  factory Error.fromJson(Map<String, dynamic> json) {
    return Error(
      code: json['code'],
      message: json['message'],
    );
  }
}

class UserData {
  final String id;
  final String username;
  final String email;
  final String currency;
  final String accessToken;
  final List<String> budgetIds;

  UserData(
      {required this.id,
      required this.username,
      required this.email,
      required this.currency,
      required this.accessToken,
      required this.budgetIds});

  factory UserData.fromJson(Map<String, dynamic> json) {
    return UserData(
      id: json['id'],
      username: json['username'],
      email: json['email'],
      currency: json['currency'],
      accessToken: json['accessToken'],
      budgetIds: List<String>.from(json['budgetIds'].map((x) => x)),
    );
  }
}
