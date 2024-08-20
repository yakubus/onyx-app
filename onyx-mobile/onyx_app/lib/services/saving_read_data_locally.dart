import 'package:onyx_app/log/logger.dart';
import 'package:shared_preferences/shared_preferences.dart';

Future<void> saveAppPreferences(String appMode, String language) async {
  final SharedPreferences prefs = await SharedPreferences.getInstance();
  await prefs.setString('appMode', appMode);
  await prefs.setString('language', language);
  logger("local save preferences: mode:$appMode, language:$language");
}

Future<void> saveLongLivedToken(String longLivedToken) async {
  final SharedPreferences prefs = await SharedPreferences.getInstance();
  await prefs.setString('longLivedToken', longLivedToken);
  logger("longlive tocken is local saved");
}

Future<String> getLongLivedToken() async {
  final SharedPreferences prefs = await SharedPreferences.getInstance();
  logger(
      "geting local saved longlivetoken: ${prefs.getString('longLivedToken')}");
  return prefs.getString('longLivedToken') ?? '';
}

Future<Map<String, String>> getAppPreferences() async {
  final SharedPreferences prefs = await SharedPreferences.getInstance();
  final String appMode = prefs.getString('appMode') ?? 'light';
  final String language = prefs.getString('language') ?? 'en';
  logger("get local saved preferences: mode:$appMode, language:$language");
  return <String, String>{'appMode': appMode, 'language': language};
}

Future<void> clearAppPreferences() async {
  final SharedPreferences prefs = await SharedPreferences.getInstance();
  await prefs.clear();
  logger("lokalsaved data cleared");
}
