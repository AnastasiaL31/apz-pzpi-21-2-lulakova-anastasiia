//
//  FunctionListView.swift
//  SmartShelter_Mobile
//
//  Created by Anastasia Lulakova on 21.05.2024.
//

import SwiftUI

struct FunctionListView: View {
    var body: some View {
        NavigationStack{
            List{
                NavigationLink(destination: AllAnimalsView()){
                    Text("Animals")
                }
                NavigationLink(destination: StorageView()){
                    Text("Store")
                }
                NavigationLink(destination: AllStaffView()){
                    Text("Staff")
                }
            }
            .navigationBarBackButtonHidden(true)
        }
    }
}

#Preview {
    FunctionListView()
}
