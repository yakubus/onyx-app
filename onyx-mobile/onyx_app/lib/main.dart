// ignore_for_file: public_member_api_docs, sort_constructors_first

import 'package:adaptive_theme/adaptive_theme.dart';
import 'package:onyx_app/services/budget/budget.dart';
import 'package:onyx_app/services/saving_read_data_locally.dart';
import 'package:onyx_app/services/user/user.dart';
import 'package:onyx_app/themes/themes.dart';
import 'package:onyx_app/services/app_settings/settings.dart';
import 'package:flutter/material.dart';
import 'package:hooks_riverpod/hooks_riverpod.dart';
import 'package:flutter_localizations/flutter_localizations.dart';
import 'package:flutter_gen/gen_l10n/app_localizations.dart';
import 'package:shadcn_ui/shadcn_ui.dart';

// Import the Shadcn UI package

import 'routing/routing.dart';

final userToken = StateProvider<String>((ref) => '');
final longLivedToken = StateProvider<String?>((ref) => null);
final isLogged = StateProvider<bool>((ref) => false);

final settingsProvider =
    AsyncNotifierProvider<Settings, SettingsData>(Settings.new);

final userServiceDataProvider =
    AsyncNotifierProvider<UserNotifier, UserServiceModel>(UserNotifier.new);

final budgetServiceDataProvider =
    AsyncNotifierProvider<BudgetNotifier, BudgetServiceModel>(
        BudgetNotifier.new);

Future<void> fetchLongLivedToken(WidgetRef ref) async {
  final token = await getLongLivedToken();
  ref.read(longLivedToken.notifier).state = token;
}

Future<void> main() async {
  WidgetsFlutterBinding.ensureInitialized();
  runApp(const ProviderScope(
    child: MyApp(),
  ));
}

checkTheme(bool setTheme) {
  if (!setTheme) {
    return lightThemes;
  }
  return darkThemes;
}

class MyApp extends HookConsumerWidget {
  final AdaptiveThemeMode? savedThemeMode;
  const MyApp({
    Key? key,
    this.savedThemeMode,
  }) : super(key: key);

  @override
  Widget build(BuildContext context, WidgetRef ref) {
    final settings = ref.watch(settingsProvider);
    final SettingsData currentSettings = settings.maybeWhen(
        orElse: () => const SettingsData(language: "en", darkMode: false),
        data: (data) => data);

    return ShadApp(
      builder: (theme, darkTheme) => ShadApp.router(
        locale: Locale(currentSettings.language, ''),
        localizationsDelegates: const [
          AppLocalizations.delegate,
          GlobalMaterialLocalizations.delegate,
          GlobalWidgetsLocalizations.delegate,
        ],
        supportedLocales: const [
          Locale('en', ''),
          Locale('pl', ''),
        ],
        routerConfig: goRouter,
        title: 'Onyx',
        theme: checkTheme(currentSettings.darkMode),
      ),
    );
  }
}
