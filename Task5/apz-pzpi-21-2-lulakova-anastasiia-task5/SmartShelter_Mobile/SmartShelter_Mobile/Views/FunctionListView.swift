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
                if HttpClient.role == "Admin" || HttpClient.role == "Doctor"{
                    NavigationLink(destination: AllAnimalsView()){
                        Text("Animals")
                    }
                }
                if HttpClient.role == "Admin" || HttpClient.role == "Storekeeper"{
                    NavigationLink(destination: StorageView()){
                        Text("Store")
                    }
                }
                if HttpClient.role == "Admin"{
                    
                    NavigationLink(destination: AllStaffView()){
                        Text("Staff")
                    }
                }
                NavigationLink(destination: SettingsView()){
                    Text("Settings")
                }
            }
            .navigationTitle("All Functions")
            .navigationBarBackButtonHidden(true)
            
            
        }
        
        .ignoresSafeArea()
        .padding()
        .background(Color(red: 250, green: 242, blue: 220))
        //.scrollContentBackground(.hidden)
        
        
    }
}

#Preview {
    FunctionListView()
}
