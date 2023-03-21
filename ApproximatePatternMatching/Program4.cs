//using System.Security.Cryptography;
//using System;

//patterns = ["computer", "survey", "hi", "hello"]
//string = "surputer" #comnuter henlo
//max_errors = 4


//def work():
//    found = []
//    for pattern in patterns:
//        states = (len(pattern) + 1) * (max_errors + 1)
//        loaded_states = [(0, 0)]

//        while loaded_states and(data:= loaded_states.pop(0)):
//            current_state, position = data
//            if current_state >= states or position >= len(string):
//                continue

//            if position == len(string) - 1 and(current_state + 1) % (len(pattern) + 1) == 0:
//                found.append(pattern)
//                break

//            try:
//                if string[position] == pattern[current_state % (len(pattern) + 1)]:
//                    loaded_states.append((current_state + 1, position + 1))
//            except:
//pass
//            loaded_states.append((current_state + len(pattern) + 2, position))
//            loaded_states.append((current_state + len(pattern) + 1, position))
//            loaded_states.append((current_state + len(pattern) + 1, position + 1))
//            loaded_states.append((current_state + len(pattern) + 2, position + 1))

//    return found

//print(work())