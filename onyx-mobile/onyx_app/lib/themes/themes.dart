import 'package:flutter/material.dart';
import 'package:shadcn_ui/shadcn_ui.dart';

ShadThemeData lightThemes = ShadThemeData(
  colorScheme: const ShadSlateColorScheme.light(
    background: Color.fromARGB(255, 255, 255, 255),
  ),
  brightness: Brightness.light,
  cardTheme:
      const ShadCardTheme(backgroundColor: Color.fromARGB(255, 35, 73, 35)),
);

ShadThemeData darkThemes = ShadThemeData(
  colorScheme: const ShadSlateColorScheme.dark(
    background: Color.fromARGB(255, 54, 56, 54),
  ),
  brightness: Brightness.dark,
  cardTheme:
      const ShadCardTheme(backgroundColor: Color.fromARGB(255, 38, 53, 38)),
);
