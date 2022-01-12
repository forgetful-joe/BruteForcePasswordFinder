# Summary

BruteForcePasswordFinder is a tool (written in .net 6) to brute force passwords for Ethereum KeyStore files.

It could be adapted to find other passwords formats (or mneumonics) if you have the right nuget package or create your own library

This tool was inspired by https://btcrecover.readthedocs.io/en/latest/ and https://github.com/ryepdx/pyethrecover, which can also be used to search ethereum keystore files (among many other formats), but it's not as flexible to express the different potential combinations.

You express the combinations in c# itself, so you can use your coding skills more easily to write more likely combinations.

# Features

## Express your combinations in a high level language (C#)

With C# it's easier (comparing to configuring text files or low level languages like c++) to write what you want, and it's free to use, you can download [Visual Studio Community Edition](https://visualstudio.microsoft.com/vs/community/) or use VSCode.
There are helps to try different word casing or remove spaces.

## Handle multiple files and multiple combinations

If you have multiple files, and you don't know which password goes in which, you can try in both at the same time! You can also express the likelyhood of each combination, so the machine will run the most likely first.

Useful to leave the machine working overnight or over the weekend.

## Made to be run in multiple machines

You can set different machines to run different combinations, each will create tracking files with the machine name in the name, so you can merge then later.

## Can be run in the cloud

.net 6 can run on windows or linux machines, setupGoogleCloud.sh has an example on how to set up the running environment in the Google Cloud. AWS and Azure would be similar.

## Won't try the same password again

Each tried and failed password is saved to a text file, before new passwords are tried, the software checks which ones it already did and won't retry.

If the software or the machine crashes, it will continue from where it last stopped when you restart the software.

## Multithreaded

It will use all the treads available on the machine

# Important concepts

# Combination vs Permutations
Combination is a set of all the possible fragments and how they could be combined. Let's say you know your password starts with a vegetable, then a main dish, then a 1 or 2.
A combination would be:
- Spinash OR Lettuce
- Stake or Pasta
- 1 or 2

This combination, when you do all the possible permutations, will result in all the different passwords we could potentially have, each one is a 'permutation':
- SpinashStake1
- SpinashPasta1
- LetuceStake2
etc

# Fragments

These are the parts of your pasword you can remember, in the example above, Spinash and Pasta are 2 possible fragments

# How to use it

1) Put your keystore file(s) in the folder 'files', change the extension to .json, all files of these extension will be picked up

2) The folder 'Secret' the only files you will need to change (probably)
    - SecretFragmentService contains the fragments you remember, put then in likelyhood of being present in the password
    - SecretCombinationService will combine the fragments into combinations

3) The method MatchesPasswordRequirements runs last, filtering OUT all the permutations permutations that are impossible, for example too few characters. Verify the wallet you use matches these requirements.

4) Run the software ! You can run directly from Visual Studio (VS), but because VS uses a lot of memory / processing, I recommend you just compile via VS and then close all the other software in your comptuer, and run the executable (it's a console application)

# FAQ

## Wouldn't it be faster to run on a low level language ?

Yes it would, but I was more confortable on C#, and I feel it's more flexible.

## Any security considerations ?

Some advice on security, but you might want to take even more precautions just in case

1)  Don't upgrade the nuget package Nethereum.KeyStore to a version different than 4.1.1, so you can't be the target of a supply chain attack. Only upgrade if you checked the code of the new version to make sure it doesn't do anything dodgy

2) Run this on a machine NOT connected to the internet, so if the software finds the password, it can be stolen in case you already have malicious software on your computer. I would recommend you write down the found password and wipe the machine clean.

## Known issues ?

1) The software crashes rarely (~2% of the time), I was not able to find the root cause, sorry.
2) Sometimes it misses trying a few password from the list, seems to skip a couple, just run it twice for the same combinations to be sure.

## Does it run in a network?

No, the software does not talk to any other computers, it was designed to run in an isolated machine. If you want to do distributed computing, you need to do manually.

## Do I need to be a techie to use this ?

You need to be confortable with coding in a high level language, if you have experience in Java, Javascript, Go or something similar you will be fine.
You are free to try it anyway, there is nothing this software can do to 'lock' or 'destroy' your assets or account, worst case scenario you won't find your password.
It's recommended you ask a developer friend of yours to help if you don't feel confortable.

## Will this repository be updated ? Can I raise issues ?

This repo will never be updated, feel free to fork it and do your own improvements

## How do I know it succeeded ?

If a password is found, it will stop running immediately, log to console and create a filename called 'success.txt' with your password inside.

## Why isn't the code better written ?

I wrote this code in a mad panic trying to find my own password, didn't have the mental energy to follow proper SOLID principles and such; sorry.

## Why is this open source ?

I considered selling this software or opening my own company to find people's passwords.

But at the end of the day, I realize that the open source community has enriched my life by quite a lot, and I decided to pay back.

## Can I sell this software ?

You can see the license under the file 'LICENSE'

## How do I remember my passwords ?

This tool can combine different potential fragments that might be in your passwords, but you need to have the fragments first.

Some things you can try to 'shake the tree of memories' and remember some more fragments:

1) Look at all your current passwords, browsers sometimes save passwords, which you can later export to a .csv to look at them all in one place
2) Create a timeline of events on your life around the time you created the password, some potential sources of insight:
- financial transactions (bank transfers, investments, withdraws)
- shopping (credit card transactions, Amazon/Ebay/Etsy... accounts)
- Photos and videos you took
- Journal or diary
- Google and youtube searches
- Videos you watched at the time
- Work projects and getaways
- Restaurants and takeaways you liked




