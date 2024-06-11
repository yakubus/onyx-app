// ignore_for_file: public_member_api_docs, sort_constructors_first
// ignore: implementation_imports
import 'package:flutter/material.dart';

import 'package:hooks_riverpod/hooks_riverpod.dart';
import 'package:shadcn_ui/shadcn_ui.dart';

class Logo extends HookConsumerWidget {
  final double size;

  const Logo({
    super.key,
    required this.size,
  });

  @override
  Widget build(BuildContext context, WidgetRef ref) {
    double scaaling = size / 50;
    return Column(
      children: [
        ShadImage.square(
          'lib\\assets\\logo_white.png',
          size: size,
        ),
        Text(
          style: TextStyle(
              color: Colors.white,
              fontWeight: FontWeight.bold,
              fontSize: 20 * scaaling),
          'ONYX',
          textAlign: TextAlign.center,
        )
      ],
    );
  }
}
