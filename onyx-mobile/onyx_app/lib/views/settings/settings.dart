// ignore_for_file: public_member_api_docs, sort_constructors_first
import 'dart:async';

import 'package:hooks_riverpod/hooks_riverpod.dart';

import 'package:onyx_app/views/settings/settings_repo.dart';

enum Language { en, pl }

class SettingsData {
  final String language;
  final bool darkMode;

  const SettingsData({
    required this.language,
    required this.darkMode,
  });
}

class Settings extends AsyncNotifier<SettingsData> {
  Future<void> refresh() async {
    state = await AsyncValue.guard(
        () => ref.watch(settingsRepoProvider).getSettings());
  }

  @override
  Future<SettingsData> build() {
    return ref.read(settingsRepoProvider).getSettings();
  }

  Future<void> saveSettings(SettingsData settings) async {
    await ref.read(settingsRepoProvider).saveSettings(settings);
    refresh();
  }
}
