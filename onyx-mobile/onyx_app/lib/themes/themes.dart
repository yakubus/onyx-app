import 'package:flutter/material.dart';
import 'package:flutter/widgets.dart';
import 'package:shadcn_ui/shadcn_ui.dart';

ShadThemeData lightThemes = ShadThemeData(
    colorScheme: const ShadSlateColorScheme.light(
      background: Color.fromARGB(255, 255, 255, 255),
      primary: Color.fromARGB(255, 255, 255, 255),
    ),
    brightness: Brightness.light,
    cardTheme:
        const ShadCardTheme(backgroundColor: Color.fromARGB(255, 35, 73, 35)),
    sheetTheme: ShadSheetTheme(
        backgroundColor: const Color.fromRGBO(52, 85, 74, 1.000),
        border: Border.all(width: 0.0),
        mainAxisAlignment: MainAxisAlignment.start));

ShadThemeData darkThemes = ShadThemeData(
  colorScheme: const ShadSlateColorScheme.dark(
    background: Color.fromARGB(255, 54, 56, 54),
    primary: Color.fromARGB(255, 54, 56, 54),
  ),
  brightness: Brightness.dark,
  cardTheme:
      const ShadCardTheme(backgroundColor: Color.fromARGB(255, 38, 53, 38)),
  sheetTheme:
      const ShadSheetTheme(backgroundColor: Color.fromRGBO(52, 85, 74, 1.000)),
);
