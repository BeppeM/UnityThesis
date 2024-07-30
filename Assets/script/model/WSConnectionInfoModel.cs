
using System;

public class WSConnectionInfoModel {

    private String url;
    private String connectionType;


    public WSConnectionInfoModel(String url, String connectionType){
        this.url = url;
        this.connectionType = connectionType;
    }

    public void setUrl(String url){
        this.url = url;
    }

    public void setConnectionType(String connectionType){
        this.connectionType = connectionType;
    }

    public String getUrl(){
        return url;
    }

    
    public String getConnectionType(){
        return connectionType;
    }

}