// ignore_for_file: public_member_api_docs, sort_constructors_first
import 'package:onyx_app/views/settings/settings.dart';
import 'package:hooks_riverpod/hooks_riverpod.dart';
import 'package:shared_preferences/shared_preferences.dart';

final settingsRepoProvider = Provider((ref) => SettingsRepo(ref));

class SettingsRepo {
  final Ref ref;

  const SettingsRepo(this.ref);

  Future<SettingsData> getSettings() async {
    final SharedPreferences prefs = await SharedPreferences.getInstance();
    final bool darkMode = prefs.getBool('darkMode') ?? false;
    final String language = prefs.getString('language') ?? 'en';
    return SettingsData(darkMode: darkMode, language: language);
  }

  Future<void> saveSettings(SettingsData settings) async {
    final SharedPreferences prefs = await SharedPreferences.getInstance();
    await prefs.setBool('darkMode', settings.darkMode);
    await prefs.setString('language', settings.language);
  }
}
