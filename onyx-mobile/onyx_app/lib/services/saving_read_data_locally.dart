import 'package:shared_preferences/shared_preferences.dart';

Future<void> saveAppPreferences(String appMode, String language) async {
  final SharedPreferences prefs = await SharedPreferences.getInstance();
  await prefs.setString('appMode', appMode);
  await prefs.setString('language', language);
}

Future<Map<String, String>> getAppPreferences() async {
  final SharedPreferences prefs = await SharedPreferences.getInstance();
  final String appMode = prefs.getString('appMode') ?? 'light';
  final String language = prefs.getString('language') ?? 'en';
  return <String, String>{'appMode': appMode, 'language': language};
}

Future<void> clearAppPreferences() async {
  final SharedPreferences prefs = await SharedPreferences.getInstance();
  await prefs.clear();
}
